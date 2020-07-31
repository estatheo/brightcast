using System.Linq;
using AutoMapper;
using brightcast.Entities;
using brightcast.Helpers;
using brightcast.Models.Campaigns;
using brightcast.Models.Contacts;
using brightcast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace brightcast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

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
            var contacts = _contactService.GetAllByContactListId(id);
            return Ok(contacts.Select(x => new ContactModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Id = x.Id,
                ContactListId = x.ContactListId,
                Email = x.Email,
                Phone = x.Phone,
                Subscribed = x.Subscribed
            }));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _contactService.GetById(id);
            var model = _mapper.Map<CampaignModel>(user);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ContactModel model)
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
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPost("new")]
        public IActionResult Create([FromBody] ContactModel model)
        {
            //todo add check if userprofile == contactlist.userprofile

            try
            {
                _contactService.Create(_mapper.Map<Contact>(model));

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
            _contactService.Delete(id);
            return Ok();
        }
    }
}