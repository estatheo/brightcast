using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using brightcast.Entities;
using brightcast.Helpers;
using brightcast.Models.Campaigns;
using brightcast.Models.ContactLists;
using brightcast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace brightcast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignController : ControllerBase
    {
        private IUserProfileService _userProfileService;
        private ICampaignService _campaignService;
        private ICampaignSentService _campaignSentService;
        private ICampaignSentStatsService _campaignSentStatsService;
        private IContactListService _contactListService;
        private IContactService _contactService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public CampaignController(
            IUserProfileService userProfileService,
            ICampaignService campaignService,
            ICampaignSentService campaignSentService,
            ICampaignSentStatsService campaignSentStatsService,
            IContactListService contactListService,
            IContactService contactService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _campaignService = campaignService;
            _campaignSentService = campaignSentService;
            _campaignSentStatsService = campaignSentStatsService;
            _userProfileService = userProfileService;
            _contactListService = contactListService;
            _contactService = contactService;
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

            var userProfile = _userProfileService.GetAllByUserId(userId).FirstOrDefault(x => x.Default );

            if (userProfile == null || userProfile.Id == 0)
            {
                return NotFound(
                    new
                    {
                        message = "UserProfile Not Found"
                    });
            }
            
            var campaigns = _campaignService.GetByUserProfileId(userProfile.Id);
            if (campaigns == null || campaigns.Count == 0)
            {
                return Ok();
            }

            var result = new List<CampaignResponseModel>();

            foreach (var campaign in campaigns)
            {
                var campaignsSent = _campaignSentService.GetAllByCampaignId(campaign.Id);
                var values = new []{0,0,0};
                if (campaignsSent != null)
                {
                    foreach (var campaignSent in campaignsSent)
                    {
                        values[0] += _contactService.GetAllByContactListId(campaignSent.ContactListId.GetValueOrDefault())
                            .Count;
                        var stats = _campaignSentStatsService.GetByCampaignSentId(campaignSent.Id);
                        if (stats != null)
                        {
                            foreach (var campaignSentStatse in stats)
                            {
                                values[1] += campaignSentStatse.Read;
                                values[2] += campaignSentStatse.Replies;
                            }
                        }
                    }
                }
                

                result.Add(new CampaignResponseModel()
                {
                    ContactListList = _contactListService.GetAllByUserProfileId(userProfile.Id).Select(x=> new ContactListModel()
                    {
                        Name = x.Name,
                        FileUrl = x.FileUrl,
                        Id = x.Id
                    }).ToList(),
                    FileUrl = campaign.FileUrl,
                    Id = campaign.Id,
                    Message = campaign.Message,
                    Name = campaign.Name,
                    Status = campaign.Status,
                    Sent = values[0],
                    Read = values[1],
                    Response = values[2]
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
        public IActionResult Update(int id, [FromBody]CampaignModel model)
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
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody]CampaignModel model)
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

            var userProfile = _userProfileService.GetAllByUserId(userId).FirstOrDefault(x => x.Default && x.Deleted == 0);

            if (userProfile == null || userProfile.Id == 0)
            {
                return NotFound(
                    new
                    {
                        message = "UserProfile Not Found"
                    });
            }

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
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("new")]
        public IActionResult CreateAndSend([FromBody]CampaignSendModel model)
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

            var userProfile = _userProfileService.GetAllByUserId(userId).FirstOrDefault(x => x.Default && x.Deleted == 0);

            if (userProfile == null || userProfile.Id == 0)
            {
                return NotFound(
                    new
                    {
                        message = "UserProfile Not Found"
                    });
            }

            try
            {
                // update user 
                var campaign = _campaignService.Create(new Campaign()
                {
                    Name = model.Name,
                    FileUrl = model.FileUrl,
                    Status = 0,
                    Message = model.Message,
                    UserProfileId = userProfile.Id
                });

                _campaignSentService.Create(new CampaignSent()
                {
                    CampaignId = campaign.Id,
                    ContactListId = model.ContactListId,
                    Date = DateTime.UtcNow
                });

                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
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
