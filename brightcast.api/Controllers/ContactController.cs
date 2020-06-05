using System.Collections.Generic;
using AutoMapper;
using brightcast.Entities;
using brightcast.Helpers;
using brightcast.Models.Campaigns;
using brightcast.Models.ContactLists;
using brightcast.Models.Contacts;
using brightcast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace brightcast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private IContactService _contactService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ContactController(
            IContactService contactService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _contactService = contactService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }


        [HttpGet("ofList/{id}/")]
        public IActionResult GetAll(int id)
        {
            var users = _contactService.GetAllByContactListId(id);
            var model = _mapper.Map<IList<ContactListModel>>(users);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _contactService.GetById(id);
            var model = _mapper.Map<CampaignModel>(user);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]ContactModel model)
        {
            // map model to entity and set id
            var contact = _mapper.Map<Contact>(model);
            contact.Id = id;

            try
            {
                // update user 
                _contactService.Update(contact);
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
            _contactService.Delete(id);
            return Ok();
        }
    }
}
