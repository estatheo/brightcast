using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core.Pipeline;
using brightcast.Entities;
using brightcast.Helpers;
using brightcast.Models.Campaigns;
using brightcast.Models.ContactLists;
using brightcast.Models.Twilio;
using brightcast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace brightcast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CampaignController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly ICampaignSentService _campaignSentService;
        private readonly ICampaignSentStatsService _campaignSentStatsService;
        private readonly ICampaignService _campaignService;
        private readonly IContactListService _contactListService;
        private readonly IContactService _contactService;
        private readonly IBusinessService _businessService;
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;
        private readonly IUserProfileService _userProfileService;

        public CampaignController(
            IUserProfileService userProfileService,
            ICampaignService campaignService,
            ICampaignSentService campaignSentService,
            ICampaignSentStatsService campaignSentStatsService,
            IContactListService contactListService,
            IContactService contactService,
            IMessageService messageService,
            IBusinessService businessService,
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
            _businessService = businessService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }


        [HttpGet("data")]
        public IActionResult GetAll(int id)
        {
            int userId;

            try
            {
                userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            }
            catch (Exception)
            {
                return BadRequest("User not found");
            }

            var userProfile = _userProfileService.GetAllByUserId(userId).FirstOrDefault(x => x.Default);

            if (userProfile == null || userProfile.Id == 0)
                return NotFound(
                    new
                    {
                        message = "UserProfile Not Found"
                    });

            var campaigns = _campaignService.GetByUserProfileId(userProfile.Id);
            if (campaigns == null || campaigns.Count == 0)
                return Ok(new
                {
                    contactLists = _contactListService.GetAllByUserProfileId(userProfile.Id).Select(x =>
                        new ContactListModel
                        {
                            Name = x.Name,
                            FileUrl = x.FileUrl,
                            Id = x.Id
                        }).ToList(),
                    campaigns = new List<CampaignResponseModel>()
                });

            var result = new
            {
                contactLists = _contactListService.GetAllByUserProfileId(userProfile.Id).Select(x =>
                    new ContactListModel
                    {
                        Name = x.Name,
                        FileUrl = x.FileUrl,
                        Id = x.Id
                    }).ToList(),
                campaigns = new List<CampaignResponseModel>()
            };

            foreach (var campaign in campaigns)
            {
                //var campaignsSent = _campaignSentService.GetAllByCampaignId(campaign.Id);
                var campaignsSent = _messageService.GetCampaignMessagesByCampaignId(campaign.Id);
                var responses = _messageService.GetReceiveMessagesByCampaignId(campaign.Id);
                var values = new[] {0, 0, 0};
                if (campaignsSent != null)
                {
                    foreach (var campaignSent in campaignsSent)
                    {
                        values[0] += campaignSent.Status == "read" || campaignSent.Status == "delivered" || campaignSent.Status == "sent" || campaignSent.Status == "received" || campaignSent.Status == "receiving" ? 1 : 0;
                        values[1] += campaignSent.Status == "read" ? 1 : 0;
                    }
                }

                if (responses != null)
                {
                    foreach (var receiveMessage in responses)
                    {
                        values[2] += receiveMessage.Status == "read" || receiveMessage.Status == "delivered" ||
                                     receiveMessage.Status == "received" || receiveMessage.Status == "receiving"
                            ? 1
                            : 0;
                    }
                }

                result.campaigns.Add(new CampaignResponseModel
                {
                    FileUrl = campaign.FileUrl,
                    Id = campaign.Id,
                    Message = campaign.Message,
                    Name = campaign.Name,
                    Status = campaign.Status,
                    Sent = values[0],
                    Read = values[1],
                    Response = values[2],
                    ContactListIds = _campaignService.GetCcl(campaign.Id).Select(x => x.ContactListId).ToList()
                });
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var campaign = _campaignService.GetById(id);
            var model = _mapper.Map<CampaignModel>(campaign);
            return Ok(model);
        }

        [HttpGet("data/{id}")]
        public IActionResult GetCampaignData(int id)
        {
            var campaign = _campaignService.GetById(id);

            var messages = _messageService.GetCampaignMessagesByCampaignId(campaign.Id);
            var receivedMessages = _messageService.GetReceiveMessagesByCampaignId(campaign.Id);
            var contactLists = _contactListService.GetByCampaignId(campaign.Id);
            var contacts = new List<Contact>();
            foreach (var contactList in contactLists)
            {
                var c = _contactService.GetAllByContactListId(contactList.Id);
                contacts.AddRange(c);
            }

            //todo: add filtering
            var model = new CampaignDataModel()
            {
                Id = campaign.Id,
                FileUrl = campaign.FileUrl,
                Name = campaign.Name,
                Message = campaign.Message,
                Status = campaign.Status,
                Delivered = messages.Count(x => x.Status == "delivered" || x.Status == "read"),
                DeliveredPercentage = 0,
                Replies = receivedMessages.Count(x => x.Status == "delivered" || x.Status == "read"),
                RepliesPercentage = 0,
                Subscribed = contacts.Count(),
                SubscribedPercentage = 0,
                Read = messages.Count(x => x.Status == "read"),
                ReadPercentage = 0
            };

            if (messages.Count(x =>
                (x.Status == "delivered" || x.Status == "read") &&
                (x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0)) &&
                 x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0)))) > 0)
            {
                model.DeliveredPercentage = messages.Count(x => (x.Status == "delivered" || x.Status == "read")
                                                                && (x.CreatedAt >=
                                                                    DateTime.Now.Subtract(new TimeSpan(28, 0, 0))
                                                                ))
                    / messages.Count(x =>
                        (x.Status == "delivered" || x.Status == "read") &&
                        (x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0)) &&
                         x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0)))) * 100;
            }

            if (receivedMessages.Count(x =>
                (x.Status == "delivered" || x.Status == "read") &&
                (x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0)) &&
                 x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0)))) > 0)
            {
                model.RepliesPercentage = receivedMessages.Count(x => (x.Status == "delivered" || x.Status == "read")
                                                                      && (x.CreatedAt >=
                                                                          DateTime.Now.Subtract(new TimeSpan(28, 0, 0))
                                                                      ))
                    / receivedMessages.Count(x =>
                        (x.Status == "delivered" || x.Status == "read") &&
                        (x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0)) &&
                         x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0)))) * 100;
            }

            if (contacts.Count(x =>
                (x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0)) &&
                 x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0)))) > 0)
            {
                model.SubscribedPercentage = contacts.Count(
                        x => x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(28, 0, 0))
                    )
                    / contacts.Count(x =>
                        (x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0)) &&
                         x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0)))) * 100;
            }

            if (messages.Count(x =>
                (x.Status == "read") &&
                (x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0)) &&
                 x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0)))) > 0)
            {
                model.ReadPercentage = messages.Count(x => (x.Status == "read")
                                                           && (x.CreatedAt >=
                                                               DateTime.Now.Subtract(new TimeSpan(28, 0, 0))
                                                           ))
                    / messages.Count(x =>
                        (x.Status == "read") &&
                        (x.CreatedAt >= DateTime.Now.Subtract(new TimeSpan(56, 0, 0)) &&
                         x.CreatedAt <= DateTime.Now.Subtract(new TimeSpan(28, 0, 0)))) * 100;
            }



            return Ok(model);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CampaignModel model)
        {
            // map model to entity and set id
            var campaign = _mapper.Map<Campaign>(model);
            campaign.Id = id;

            try
            {
                // update user 
                _campaignService.Update(campaign);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] CampaignModel model)
        {
            int userId;

            try
            {
                userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            }
            catch (Exception)
            {
                return BadRequest("User not found");
            }

            var userProfile = _userProfileService.GetAllByUserId(userId)
                .FirstOrDefault(x => x.Default && x.Deleted == 0);

            if (userProfile == null || userProfile.Id == 0)
                return NotFound(
                    new
                    {
                        message = "UserProfile Not Found"
                    });

            // map model to entity and set id
            var campaign = _mapper.Map<Campaign>(model);
            campaign.UserProfileId = userProfile.Id;
            try
            {
                // update user 
                _campaignService.Create(campaign);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [AllowAnonymous]
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] CampaignSendModel model)
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
                var contactLists = _contactListService.GetByCampaignId(model.Id);

                //todo: change with a loop for each contact list
                var contacts = _contactService.GetAllSubscribedByContactListId(contactLists[0].Id);

                var business = _businessService.GetById(userProfile.BusinessId);

                foreach (var contact in contacts)
                {
                    var client = new HttpClient();

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
                        {Content = requestModel};

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
                        CampaignId = model.Id,
                        Status = resultModel.Status
                    });

                   
                }

                var campaign = _campaignService.GetById(model.Id);

                campaign.Status = 2;

                _campaignService.Update(campaign);

                var resultCampaign = new CampaignModel()
                {
                    Id = campaign.Id,
                    Status = campaign.Status,
                    Message = campaign.Message,
                    FileUrl = campaign.FileUrl,
                    Name = campaign.Name
                };

                return Ok(resultCampaign);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPost("new")]
        public IActionResult CreateAndSend([FromBody] CampaignSendModel model)
        {
            int userId;

            try
            {
                userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            }
            catch (Exception)
            {
                return BadRequest("User not found");
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
                var campaign = _campaignService.Create(new Campaign
                {
                    Name = model.Name,
                    FileUrl = model.FileUrl,
                    Status = 0,
                    Message = model.Message,
                    UserProfileId = userProfile.Id
                });

                var contactList = _contactListService.GetById(model.ContactListIds.FirstOrDefault());

                _campaignService.Add(new CampaignContactList
                {
                    Campaign = campaign,
                    CampaignId = campaign.Id,
                    ContactList = contactList,
                    ContactListId = model.ContactListIds.FirstOrDefault()
                });

                var result = new CampaignModel()
                {
                    Id = campaign.Id,
                    Status = campaign.Status,
                    Message = campaign.Message,
                    FileUrl = campaign.FileUrl,
                    Name = campaign.Name
                };

                return Ok(result);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _campaignService.Delete(id);
            return Ok();
        }
    }
}