using System.Collections.Generic;
using AutoMapper;
using brightcast.Entities;
using brightcast.Helpers;
using brightcast.Models.Campaigns;
using brightcast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace brightcast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CampaignController : ControllerBase
    {
        private ICampaignService _campaignService;
        private ICampaignSentService _campaignSentService;
        private ICampaignSentStatsService _campaignSentStatsService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public CampaignController(
            ICampaignService campaignService,
            ICampaignSentService campaignSentService,
            ICampaignSentStatsService campaignSentStatsService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _campaignService = campaignService;
            _campaignSentService = campaignSentService;
            _campaignSentStatsService = campaignSentStatsService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }


        [HttpGet("ofUser/{id}/")]
        public IActionResult GetAll(int id)
        {
            var users = _campaignService.GetByUserProfileId(id);
            var model = _mapper.Map<IList<CampaignModel>>(users);
            return Ok(model);
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _campaignService.Delete(id);
            return Ok();
        }
    }
}
