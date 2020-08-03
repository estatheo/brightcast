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
                var campaignsSent = _campaignSentService.GetAllByCampaignId(campaign.Id);
                var values = new[] {0, 0, 0};
                if (campaignsSent != null)
                    foreach (var campaignSent in campaignsSent)
                    {
                        values[0] += _contactService.GetAllByContactListId(campaignSent.ContactListId)
                            .Count;
                        var stats = _campaignSentStatsService.GetByCampaignSentId(campaignSent.Id);
                        if (stats != null)
                            foreach (var campaignSentStatse in stats)
                            {
                                values[1] += campaignSentStatse.Read;
                                values[2] += campaignSentStatse.Replies;
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
            var user = _campaignService.GetById(id);
            var model = _mapper.Map<CampaignModel>(user);
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
                //todo: change with a loop for each contact list
                var contacts = _contactService.GetAllSubscribedByContactListId(model.ContactListIds.FirstOrDefault());

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

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
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

                return Ok();
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