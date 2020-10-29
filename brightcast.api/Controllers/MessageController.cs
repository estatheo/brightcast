using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using brightcast.Entities;
using brightcast.Helpers;
using brightcast.Models.Chats;
using brightcast.Models.Twilio;
using brightcast.Services;
using ChatApp.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace brightcast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private ICampaignSentService _campaignSentService;
        private ICampaignSentStatsService _campaignSentStatsService;
        private readonly ICampaignService _campaignService;
        private IContactListService _contactListService;
        private IContactService _contactService;
        private IMapper _mapper;
        private readonly IMessageService _messageService;
        private IUserProfileService _userProfileService;
        private readonly IHubContext<ChatHub> _hub;
        private IChatService _chatService;

        public MessageController(
            IUserProfileService userProfileService,
            ICampaignService campaignService,
            ICampaignSentService campaignSentService,
            ICampaignSentStatsService campaignSentStatsService,
            IContactListService contactListService,
            IContactService contactService,
            IMessageService messageService,
            IChatService chatService,
            IHubContext<ChatHub> hub,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _campaignService = campaignService;
            _campaignSentService = campaignSentService;
            _campaignSentStatsService = campaignSentStatsService;
            _userProfileService = userProfileService;
            _contactListService = contactListService;
            _contactService = contactService;
            _messageService = messageService;
            _chatService = chatService;
            _hub = hub;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("callback/template")]
        public IActionResult CallbackForTemplateMessage([FromForm] TwilioWhatsappCallbackModel model)
        {
            try
            {
                // todo test concurrency and find a better way to match to correct campaign & contact

                var template = _messageService.GetTemplateMessageByMessageId(model.MessageSid);

                template.Status = model.MessageStatus;
                template.Error_Code = model.ErrorCode;
                template.Error_Message = model.ErrorMessage;
                //template.Date_Created = model.DateCreated;
                //template.Date_Updated = model.DateUpdated;
                //template.Date_Sent = model.DateSent;

                _messageService.UpdateTemplateMessage(template);

                //todo add logic to check for void error codes and unsubscribe from list


                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [AllowAnonymous]
        [HttpPost("receiveMessage")]
        public async Task<IActionResult> ReceiveMessage([FromForm] TwilioWhatsappCallbackModel model)
        {
            try
            {
                var templateMessage = _messageService.GetLastByTo(model.From);

                var campaign = _campaignService.GetById(templateMessage.CampaignId);
                var contact = _contactService.GetById(templateMessage.ContactId);

                if (model.ErrorCode == null && _messageService.CheckReceivedCampaignMessage(templateMessage) == false)
                {
                    _messageService.AddReceiveMessage(new ReceiveMessage
                    {
                        Body = model.Body,
                        Date_Created = DateTime.UtcNow,
                        Error_Code = model.ErrorCode,
                        Error_Message = model.ErrorMessage,
                        From = model.From,
                        To = model.To,
                        MessageSid = model.MessageSid,
                        Status = model.MessageStatus,
                        CampaignId = templateMessage.CampaignId,
                        ContactId = templateMessage.ContactId
                    });

                    var chatMessage = new ChatMessage()
                    {
                        Text = model.Body,
                        CreatedAt = DateTime.Now,
                        Reply = true,
                        Type = "text",
                        Files = "",
                        AvatarUrl = "",
                        SenderId = contact.Id,
                        SenderName = contact.FirstName + " " + contact.LastName,
                        CampaignId = templateMessage.CampaignId,
                        ContactId = templateMessage.ContactId
                    };

                    _chatService.Create(chatMessage);
                    
                    await _hub.Clients.All.SendAsync("newMessage", chatMessage);

                    //send campaign message

                    var client = new HttpClient();
                    FormUrlEncodedContent requestModel = null;


                    if (string.IsNullOrWhiteSpace(campaign.FileUrl))
                    {
                        requestModel = new FormUrlEncodedContent(
                            new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("From", $"{_appSettings.TwilioWhatsappNumber}"),
                                new KeyValuePair<string, string>("Body", $"{campaign.Message}"),
                                new KeyValuePair<string, string>("StatusCallback",
                                    $"{_appSettings.ApiBaseUrl}/message/callback/campaign"),
                                new KeyValuePair<string, string>("To", $"{model.From}")
                            }
                        );
                    }
                    else
                    {
                        requestModel = new FormUrlEncodedContent(
                            new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("From", $"{_appSettings.TwilioWhatsappNumber}"),
                                new KeyValuePair<string, string>("Body", $"{campaign.Message}"),
                                new KeyValuePair<string, string>("MediaUrl", $"{campaign.FileUrl}"),
                                new KeyValuePair<string, string>("StatusCallback",
                                    $"{_appSettings.ApiBaseUrl}/message/callback/campaign"),
                                new KeyValuePair<string, string>("To", $"{model.From}")
                            }
                        );
                    }


                    
                    var req = new HttpRequestMessage(HttpMethod.Post,
                            $"https://api.twilio.com/2010-04-01/Accounts/{_appSettings.TwilioAccountSID}/Messages.json")
                        {Content = requestModel};

                    req.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            $"{_appSettings.TwilioAccountSID}:{_appSettings.TwilioAuthToken}")));

                    var result = await client.SendAsync(req);

                    var resultModel =
                        JsonConvert.DeserializeObject<TwilioTemplateMessageModel>(
                            await result.Content.ReadAsStringAsync());

                    _messageService.AddCampaignMessage(new CampaignMessage
                    {
                        MessageSid = resultModel.Sid,
                        Body = resultModel.Body,
                        Date_Created = DateTime.UtcNow,
                        Error_Code = resultModel.Error_Code,
                        Error_Message = resultModel.Error_Message,
                        From = resultModel.From,
                        To = resultModel.To,
                        ContactId = templateMessage.ContactId,
                        CampaignId = templateMessage.CampaignId
                    });

                    var chatModel = new ChatMessage()
                    {
                        Text = resultModel.Body,
                        CreatedAt = DateTime.Now,
                        Reply = true,
                        Type = "text",
                        Files = string.IsNullOrWhiteSpace(campaign.FileUrl) ? "" : campaign.FileUrl,
                        AvatarUrl = "",
                        SenderId = contact.Id,
                        SenderName = contact.FirstName + " " + contact.LastName,
                        CampaignId = templateMessage.CampaignId,
                        ContactId = templateMessage.ContactId
                    };

                    _chatService.Create(chatModel);

                    await _hub.Clients.All.SendAsync("newMessage", chatModel);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [AllowAnonymous]
        [HttpPost("callback/campaign")]
        public IActionResult CallbackForCampaignMessage([FromForm] TwilioWhatsappCallbackModel model)
        {
            try
            {
                var template = _messageService.GetCampaignMessageByMessageId(model.MessageSid);

                if (template != null)
                {
                    template.Status = model.MessageStatus;
                    template.Error_Code = model.ErrorCode;
                    template.Error_Message = model.ErrorMessage;
                    template.Date_Created = model.DateCreated;
                    template.Date_Updated = model.DateUpdated;
                    template.Date_Sent = model.DateSent;

                    _messageService.UpdateCampaignMessage(template);
                }
                else
                {
                    var templateMessage = _messageService.GetLastByTo(model.To);

                    _messageService.AddCampaignMessage(new CampaignMessage
                    {
                        Body = model.Body,
                        To = model.To,
                        From = model.From,
                        Status = model.MessageStatus,
                        MessageSid = model.MessageSid,
                        Date_Sent = model.DateSent,
                        Date_Updated = model.DateUpdated,
                        Date_Created = model.DateCreated,
                        Error_Code = model.ErrorCode,
                        Error_Message = model.ErrorMessage,
                        CampaignId = templateMessage.CampaignId,
                        ContactId = templateMessage.ContactId
                    });

                }

                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}
