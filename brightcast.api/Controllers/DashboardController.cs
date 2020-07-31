using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using brightcast.Entities;
using brightcast.Helpers;
using brightcast.Models.Dashboard;
using brightcast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace brightcast.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly ICampaignSentService _campaignSentService;
        private readonly IContactListService _contactListService;
        private readonly IContactService _contactService;
        private readonly ICampaignSentStatsService _campaignSentStatsService;
        private readonly ICampaignService _campaignService;
        private readonly IMessageService _messageService;
        private readonly IUserProfileService _userProfileService;
        private DataMappingHelper dataMapper;

        private IMapper _mapper;
        private IUserService _userService;

        private readonly DashboardDataResponse _emptyResponse = new DashboardDataResponse
        {
            Delivered = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] { DateTime.UtcNow.ToString("d") },
                ChartValues = new[] { 0 }
            },
            Read = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] { DateTime.UtcNow.ToString("d") },
                ChartValues = new[] { 0 }
            },
            NewSubscribers = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] { DateTime.UtcNow.ToString("d") },
                ChartValues = new[] { 0 }
            },
            Unsubscribed = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] { DateTime.UtcNow.ToString("d") },
                ChartValues = new[] { 0 }
            },
            Replies = new CardStatModel
            {
                Value = 0,
                Percentage = 0,
                ChartPoints = 1,
                ChartLabels = new[] { DateTime.UtcNow.ToString("d") },
                ChartValues = new[] { 0 }
            }
        };


        public DashboardController(
            IContactListService contactListService,
            IContactService contactService,
            IMessageService messageService,
            ICampaignService campaignService,
            ICampaignSentService campaignSentService,
            ICampaignSentStatsService campaignSentStatsService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IUserService userService,
            IUserProfileService userProfileService)
        {
            _campaignService = campaignService;
            _campaignSentService = campaignSentService;
            _campaignSentStatsService = campaignSentStatsService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _userService = userService;
            _userProfileService = userProfileService;
            _messageService = messageService;
            _contactListService = contactListService;
            _contactService = contactService;

            dataMapper = new DataMappingHelper();
        }

        [HttpGet("data")]
        public IActionResult GetDataByUserId()
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


            var campaignMessages = new List<CampaignMessage>();
            var replies = new List<ReceiveMessage>();

            var campaigns = _campaignService.GetByUserProfileId(userProfile.Id);
            if (campaigns == null || campaigns.Count == 0) return Ok(_emptyResponse);

            foreach (var campaign in campaigns.Where(x => x.Deleted == 0))
            {
                campaignMessages.AddRange(_messageService.GetCampaignMessagesByCampaignId(campaign.Id));
                replies.AddRange(_messageService.GetReceiveMessagesByCampaignId(campaign.Id));
            }

            var contacts = new List<Contact>();

            var contactLists = _contactListService.GetAllByUserProfileId(userProfile.Id);

            foreach (var contactList in contactLists)
            {
                contacts.AddRange(_contactService.GetAllByContactListId(contactList.Id));
            }

            var response = dataMapper.MapStats(campaignMessages, contacts, replies);
            
            return Ok(response);
        }


        
    }
}
