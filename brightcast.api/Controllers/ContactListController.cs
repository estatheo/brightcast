using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        private readonly AppSettings _appSettings;
        private readonly ICampaignService _campaignService;
        private readonly IContactListService _contactListService;
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;
        private readonly IUserProfileService _userProfileService;
        private readonly CsvParser parser;

        public ContactListController(
            IUserProfileService userProfileService,
            IContactListService contactListService,
            ICampaignService campaignService,
            IContactService contactService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userProfileService = userProfileService;
            _contactListService = contactListService;
            _campaignService = campaignService;
            _contactService = contactService;
            _mapper = mapper;
            _appSettings = appSettings.Value;

            parser = new CsvParser(appSettings);
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

            var contactLists = _contactListService.GetAllByUserProfileId(userProfile.Id);
            if (contactLists == null || contactLists.Count == 0) return Ok();

            var result = new List<ContactListResponseModel>();

            foreach (var contactList in contactLists)
            {
                var listOfContacts = _contactService.GetAllByContactListId(contactList.Id).ToList();

                result.Add(new ContactListResponseModel
                {
                    Id = contactList.Id,
                    Name = contactList.Name,
                    KeyString = contactList.KeyString,
                    Contacts = listOfContacts.Count,
                    Unsubscribed = listOfContacts.Where(x => !x.Subscribed).ToList().Count,
                    Campaigns = _campaignService.GetByContactListId(contactList.Id).Count,
                    fileUrl = contactList.FileUrl
                });
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var contactList = _contactListService.GetById(id);
            var listOfContacts = _contactService.GetAllByContactListId(contactList.Id).ToList();

            var model = new ContactListResponseModel{
                Id = contactList.Id,
                Name = contactList.Name,
                KeyString = contactList.KeyString,
                Contacts = listOfContacts.Count,
                Unsubscribed = listOfContacts.Where(x => !x.Subscribed).ToList().Count,
                Campaigns = _campaignService.GetByContactListId(contactList.Id).Count,
                fileUrl = contactList.FileUrl
            };
            return Ok(model);
        }

        [HttpGet("key/{keyString}")]
        public IActionResult GetId(string keyString)
        {
            return Ok(_contactListService.GetIdByKeyString(keyString));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ContactListModel model)
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
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPost("new")]
        public async Task<IActionResult> Create(ContactListModel model)
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
            var contactList = _mapper.Map<ContactList>(model);
            contactList.UserProfileId = userProfile.Id;

            try
            {
                // update user 
                var entity = _contactListService.Create(contactList);

                var contacts = await parser.ParseFile(contactList.FileUrl);

                contacts.ForEach(x => _contactService.Create(new Contact
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ContactListId = entity.Id,
                    Email = x.Email,
                    Phone = x.Phone,
                    Subscribed = x.Subscribed
                }));

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
            _contactListService.Delete(id);
            return Ok();
        }
    }
}