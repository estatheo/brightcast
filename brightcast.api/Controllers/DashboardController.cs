﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using brightcast.Entities;
using brightcast.Helpers;
using brightcast.Models.Campaigns;
using brightcast.Models.Dashboard;
using brightcast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace brightcast.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private IUserService _userService;
        private IUserProfileService _userProfileService;
        private ICampaignService _campaignService;
        private ICampaignSentService _campaignSentService;
        private ICampaignSentStatsService _campaignSentStatsService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public DashboardController (
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
        }
        
        [HttpGet("data")]
        public IActionResult GetDataByUserId()
        {
            int userId;

            try
            {
                userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            } 
            catch(Exception)
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

            var campaignSentStats = new List<CampaignSentStats>();

            var campaigns = _campaignService.GetByUserProfileId(userProfile.Id);
            if (campaigns == null || campaigns.Count == 0)
            {
                return Ok(MapStats(new List<CampaignSentStats>()));
            }
            foreach (var campaign in campaigns.Where(x => x.Deleted == 0))
            {
                var campaignsSent = _campaignSentService.GetAllByCampaignId(campaign.Id);
                if (campaignsSent == null || campaignsSent.Count == 0)
                {
                    return Ok(MapStats(new List<CampaignSentStats>()));
                }
                foreach (var campaignSent in campaignsSent.Where(x => x.Deleted == 0))
                {
                    var campaignSentStatsList = _campaignSentStatsService.GetByCampaignSentId(campaignSent.Id);
                    if (campaignSentStatsList == null || campaignSentStatsList.Count == 0)
                    {
                        return Ok(MapStats(new List<CampaignSentStats>()));
                    }

                    campaignSentStats.AddRange(campaignSentStatsList.Where(x => x.Deleted == 0));
                }
            }

            var response = MapStats(campaignSentStats);
            
            return Ok(response);
        }


        public DashboardDataResponse MapStats(List<CampaignSentStats> list)
        {
            var result = new DashboardDataResponse();

            result.Delivered = new CardStatModel()
            {
                Value = list.Sum(x => x.Delivered),
                Percentage = (float)((list.Sum(x => x.Delivered) - list
                    .Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                    .Sum(x => x.Delivered)) * 100) / (float)list
                    .Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                    .Sum(x => x.Delivered),
                ChartPoints = 7,
                ChartValues = new int[] {list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date)
                    .Sum(x => x.Delivered),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date)
                    .Sum(x => x.Delivered),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date)
                    .Sum(x => x.Delivered),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date)
                    .Sum(x => x.Delivered),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date)
                    .Sum(x => x.Delivered), 
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                    .Sum(x => x.Delivered), 
                    list.Sum(x => x.Delivered)},
                ChartLabels = new string[]
                {
                    DateTime.UtcNow.Subtract(new TimeSpan(6,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(5,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(4,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(3,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(2,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(1,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.DayOfWeek.ToString()
                }
            };

            result.Read = new CardStatModel()
            {
                Value = list.Sum(x => x.Read),
                Percentage = list.Sum(x => x.Read) - list
                    .Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                    .Sum(x => x.Read),
                ChartPoints = 7,
                ChartValues = new int[] {list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date)
                        .Sum(x => x.Read),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date)
                        .Sum(x => x.Read),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date)
                        .Sum(x => x.Read),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date)
                        .Sum(x => x.Read),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date)
                        .Sum(x => x.Read),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                        .Sum(x => x.Read),
                    list.Sum(x => x.Read)},
                ChartLabels = new string[]
                {
                    DateTime.UtcNow.Subtract(new TimeSpan(6,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(5,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(4,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(3,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(2,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(1,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.DayOfWeek.ToString()
                }
            };

            result.NewSubscribers = new CardStatModel()
            {
                Value = list.Sum(x => x.NewSubscriber),
                Percentage = list.Sum(x => x.NewSubscriber) - list
                    .Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                    .Sum(x => x.NewSubscriber),
                ChartPoints = 7,
                ChartValues = new int[] {list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date)
                        .Sum(x => x.NewSubscriber),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date)
                        .Sum(x => x.NewSubscriber),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date)
                        .Sum(x => x.NewSubscriber),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date)
                        .Sum(x => x.NewSubscriber),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date)
                        .Sum(x => x.NewSubscriber),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                        .Sum(x => x.NewSubscriber),
                    list.Sum(x => x.NewSubscriber)},
                ChartLabels = new string[]
                {
                    DateTime.UtcNow.Subtract(new TimeSpan(6,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(5,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(4,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(3,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(2,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(1,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.DayOfWeek.ToString()
                }
            };

            result.Unsubscribed = new CardStatModel()
            {
                Value = list.Sum(x => x.Unsubscribed),
                Percentage = list.Sum(x => x.Unsubscribed) - list
                    .Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                    .Sum(x => x.Unsubscribed),
                ChartPoints = 7,
                ChartValues = new int[] {list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date)
                        .Sum(x => x.Unsubscribed),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date)
                        .Sum(x => x.Unsubscribed),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date)
                        .Sum(x => x.Unsubscribed),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date)
                        .Sum(x => x.Unsubscribed),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date)
                        .Sum(x => x.Unsubscribed),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                        .Sum(x => x.Unsubscribed),
                    list.Sum(x => x.Unsubscribed)},
                ChartLabels = new string[]
                {
                    DateTime.UtcNow.Subtract(new TimeSpan(6,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(5,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(4,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(3,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(2,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(1,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.DayOfWeek.ToString()
                }
            };

            result.Replies = new CardStatModel()
            {
                Value = list.Sum(x => x.Replies),
                Percentage = list.Sum(x => x.Replies) - list
                    .Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                    .Sum(x => x.Replies),
                ChartPoints = 7,
                ChartValues = new int[] {list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(6, 0, 0, 0)).Date)
                        .Sum(x => x.Replies),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(5, 0, 0, 0)).Date)
                        .Sum(x => x.Replies),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(4, 0, 0, 0)).Date)
                        .Sum(x => x.Replies),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0, 0)).Date)
                        .Sum(x => x.Replies),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Date)
                        .Sum(x => x.Replies),
                    list.Where(x => x.Date <= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)).Date)
                        .Sum(x => x.Replies),
                    list.Sum(x => x.Replies)},
                ChartLabels = new string[]
                {
                    DateTime.UtcNow.Subtract(new TimeSpan(6,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(5,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(4,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(3,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(2,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.Subtract(new TimeSpan(1,0,0,0)).DayOfWeek.ToString(),
                    DateTime.UtcNow.DayOfWeek.ToString()
                }
            };

            return result;
        }

    }
}