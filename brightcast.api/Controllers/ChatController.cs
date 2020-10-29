using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Security.Claims;
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
    public class ChatController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly IBusinessService _businessService;
        private readonly IUserProfileService _userProfileService;
        private readonly IContactService _contactService;
        private readonly IHubContext<ChatHub> _hub;
        private readonly IMapper _mapper;

        public ChatController(
            IChatService chatService,
            IContactService contactService,
            IMessageService messageService,
            IUserProfileService userProfileService,
            IBusinessService businessService,
            IHubContext<ChatHub> hub,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _chatService = chatService;
            _contactService = contactService;
            _messageService = messageService;
            _userProfileService = userProfileService;
            _businessService = businessService;
            _hub = hub;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }


        [HttpGet("ofCampaignAndContact/{campaignId}/{contactId}")]
        public IActionResult GetAllByCampaignIdAndContactId(int campaignId, int contactId)
        {
            var chatMessages = _chatService.GetAllByCampaignAndContactId(campaignId, contactId);
            return Ok(chatMessages.Select(x => new ChatModel
            {
                Id = x.Id,
                SenderId = x.SenderId,
                SenderName = x.SenderName,
                AvatarUrl = x.AvatarUrl,
                Type = x.Type,
                Files = x.Files,
                Text = x.Text,
                CreatedAt = x.CreatedAt,
                ContactId = x.ContactId,
                CampaignId = x.CampaignId,
            }));
        }

        [HttpPost("new")]
        public async Task<IActionResult> Create([FromBody] ChatModel model)
        {
            try
            {
                var contact = _contactService.GetById(model.ContactId);

                var client = new HttpClient();

                FormUrlEncodedContent requestModel;

                if (string.IsNullOrWhiteSpace(model.Files))
                {
                    requestModel = new FormUrlEncodedContent(
                        new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("From", $"{_appSettings.TwilioWhatsappNumber}"),
                            new KeyValuePair<string, string>("Body", $"{model.Text}"),
                            //new KeyValuePair<string, string>("StatusCallback",
                            //    $"{_appSettings.ApiBaseUrl}/message/callback/template"),
                            new KeyValuePair<string, string>("To", $"whatsapp:{contact.Phone}")
                        }
                    );
                }
                else
                {
                    requestModel = new FormUrlEncodedContent(
                        new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("From", $"{_appSettings.TwilioWhatsappNumber}"),
                            new KeyValuePair<string, string>("Body", $"{model.Text}"),
                            new KeyValuePair<string, string>("MediaUrl", $"{model.Files}"),
                            //new KeyValuePair<string, string>("StatusCallback",
                            //    $"{_appSettings.ApiBaseUrl}/message/callback/template"),
                            new KeyValuePair<string, string>("To", $"whatsapp:{contact.Phone}")
                        }
                    );
                }

                
                var req = new HttpRequestMessage(HttpMethod.Post,
                        $"https://api.twilio.com/2010-04-01/Accounts/{_appSettings.TwilioAccountSID}/Messages.json")
                    { Content = requestModel };

                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        $"{_appSettings.TwilioAccountSID}:{_appSettings.TwilioAuthToken}")));

                var result = await client.SendAsync(req);

                var resultModel =
                    JsonConvert.DeserializeObject<TwilioTemplateMessageModel>(
                        await result.Content.ReadAsStringAsync());
                
                _chatService.Create(_mapper.Map<ChatMessage>(model));

                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPost("sendInvitation")]
        public async Task<IActionResult> SendInvitation([FromBody] ChatModel model)
        {
            int userId;

            try
            {
                userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "User not found" });
            }

            var userProfile = _userProfileService.GetAllByUserId(userId)
                .FirstOrDefault(x => x.Default && x.Deleted == 0);

            if (userProfile == null || userProfile.Id == 0)
                return NotFound(
                    new
                    {
                        message = "UserProfile Not Found"
                    });

            try
            {
                var client = new HttpClient();

                var contact = _contactService.GetById(model.ContactId);

                var business = _businessService.GetByUserProfileId(userProfile.Id);

                var requestModel = new FormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>
                    {
                            new KeyValuePair<string, string>("From", $"{_appSettings.TwilioWhatsappNumber}"),
                            new KeyValuePair<string, string>("Body", $"{_appSettings.TwilioTemplateMessage.Replace("{{1}}", business.Name)}"),
                            new KeyValuePair<string, string>("StatusCallback",
                                $"{_appSettings.ApiBaseUrl}/message/callback/template"),
                            new KeyValuePair<string, string>("To", $"whatsapp:{contact.Phone}")
                    }
                );
                var req = new HttpRequestMessage(HttpMethod.Post,
                        $"https://api.twilio.com/2010-04-01/Accounts/{_appSettings.TwilioAccountSID}/Messages.json")
                { Content = requestModel };

                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        $"{_appSettings.TwilioAccountSID}:{_appSettings.TwilioAuthToken}")));

                var result = await client.SendAsync(req);

                var resultModel =
                    JsonConvert.DeserializeObject<TwilioTemplateMessageModel>(
                        await result.Content.ReadAsStringAsync());
                
                _messageService.AddTemplateMessage(new TemplateMessage
                {
                    MessageSid = resultModel.Sid,
                    Body = resultModel.Body,
                    Date_Created = resultModel.Date_Created,
                    Date_Sent = resultModel.Date_Sent,
                    Date_Updated = resultModel.Date_Updated,
                    Error_Code = resultModel.Error_Code,
                    Error_Message = resultModel.Error_Message,
                    From = resultModel.From,
                    To = resultModel.To,
                    ContactId = contact.Id,
                    CampaignId = model.CampaignId,
                    Status = resultModel.Status
                });

                await _hub.Clients.All.SendAsync("newMessage", model );

                _chatService.Create(_mapper.Map<ChatMessage>(model));

                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _chatService.Delete(id);
            return Ok();
        }
    }
}