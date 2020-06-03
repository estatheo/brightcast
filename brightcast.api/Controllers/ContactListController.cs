using System.Collections.Generic;
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
    [Route("[controller]")]
    public class ContactListController : ControllerBase
    {
        private IContactListService _contactListService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ContactListController(
            IContactListService contactListService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _contactListService = contactListService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }


        [HttpGet("ofUser/{id}/")]
        public IActionResult GetAll(int id)
        {
            var users = _contactListService.GetAllByUserProfileId(id);
            var model = _mapper.Map<IList<CampaignModel>>(users);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _contactListService.GetById(id);
            var model = _mapper.Map<CampaignModel>(user);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]ContactListModel model)
        {
            // map model to entity and set id
            var contactList = _mapper.Map<ContactList>(model);
            contactList.Id = id;

            try
            {
                // update user 
                _contactListService.Update(contactList);
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
            _contactListService.Delete(id);
            return Ok();
        }
    }
}
