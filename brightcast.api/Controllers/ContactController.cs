using System.Collections.Generic;
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
        private readonly IContactListService _contactListService;
        private readonly IMessageService _messageService;
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;

        public ContactController(
            IContactService contactService,
            IContactListService contactListService,
            IChatService chatService,
            IMessageService messageService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _contactService = contactService;
            _contactListService = contactListService;
            _messageService = messageService;
            _chatService = chatService;
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

        [HttpGet("byCampaignId/{id}")]
        public IActionResult GetByCampaignId(int campaignId)
        {
            var contactLists = _contactListService.GetByCampaignId(campaignId);
            
            List<ContactMessageModel> result = new List<ContactMessageModel>();
            foreach (var contactList in contactLists)
            {
                var contacts = _contactService.GetAllByContactListId(contactList.Id);

                foreach (var contactEntity in contacts)
                {
                    var lastChatMessage = _chatService.GetAllByCampaignAndContactId(campaignId, contactEntity.Id).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                    if (lastChatMessage != null)
                    {
                        result.Add(new ContactMessageModel()
                        {
                            Id = contactEntity.Id,
                            FirstName = contactEntity.FirstName,
                            LastName = contactEntity.LastName,
                            Body = lastChatMessage.Text,
                            Time = lastChatMessage.CreatedAt
                        });
                    }
                    else
                    {
                        var lastTemplateMessage = _messageService.GetCampaignMessagesByCampaignId(campaignId)
                            .Where(x => x.ContactId == contactEntity.Id).OrderByDescending(x => x.CreatedAt).FirstOrDefault();

                        result.Add(new ContactMessageModel()
                        {
                            Id = contactEntity.Id,
                            FirstName = contactEntity.FirstName,
                            LastName = contactEntity.LastName,
                            Body = lastTemplateMessage.Body,
                            Time = lastTemplateMessage.CreatedAt
                        });
                    }
                }
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var contact = _contactService.GetById(id);
            var model = new ContactModel()
            {
                ContactListId = contact.ContactListId,
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                Phone = contact.Phone,
                Subscribed = contact.Subscribed
            };

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
                var response = _contactService.Create(_mapper.Map<Contact>(model));

                return Ok(new ContactModel()
                {
                    Id = response.Id,
                    Subscribed = response.Subscribed,
                    LastName = response.LastName,
                    Phone = response.Phone,
                    Email = response.Email,
                    ContactListId = response.ContactListId,
                    FirstName = response.FirstName
                });
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