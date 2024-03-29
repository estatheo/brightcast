﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using brightcast.Entities;
using brightcast.Helpers;
using brightcast.Models.Businesses;
using brightcast.Models.Onboarding;
using brightcast.Models.UserProfiles;
using brightcast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace brightcast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IBusinessService _businessService;
        private readonly ICampaignService _campaignService;
        private readonly IContactListService _contactListService;
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;
        private readonly IUserProfileService _userProfileService;
        private readonly IUserService _userService;
        private readonly CsvParser parser;
        private IRoleService _roleService;

        public UserProfileController(
            IUserService userService,
            IUserProfileService userProfileService,
            IMapper mapper,
            IRoleService roleService,
            IContactListService contactListService,
            IContactService contactService,
            ICampaignService campaignService,
            IBusinessService businessService,
            IOptions<AppSettings> appSettings
        )
        {
            _userService = userService;
            _userProfileService = userProfileService;
            _roleService = roleService;
            _contactListService = contactListService;
            _contactService = contactService;
            _campaignService = campaignService;
            _businessService = businessService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            parser = new CsvParser(appSettings);
        }

        [HttpGet("onboardingCheck")]
        public IActionResult OnBoardingCheck()
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

            try
            {
                var userProfile = _userProfileService.GetAllByUserId(userId)
                    .FirstOrDefault(x => x.Default && x.Deleted == 0);
                if (userProfile == null || userProfile.Id == 0)
                    return Ok(
                        new
                        {
                            onboard = true
                        });
            }
            catch (Exception)
            {
                return Ok(
                    new
                    {
                        onboard = true
                    });
            }

            return Ok(
                new
                {
                    onboard = false
                });
        }

        [HttpPost("onboarding")]
        public async Task<IActionResult> RegisterProfile([FromBody] OnboardingRegistrationModel model)
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


            try
            {
                var businessModel = _mapper.Map<Business>(model.Business);
                //var roleModel = _mapper.Map<Role>(model.Role);
                var contactListModel = _mapper.Map<ContactList>(model.ContactList);
                var campaignModel = _mapper.Map<Campaign>(model.Campaign);

                var businessId = _businessService.Create(businessModel).Id;
                //var roleId = _roleService.Create(roleModel).Id;

                //map model to entity
                var profile = _mapper.Map<UserProfile>(model.UserProfile);
                profile.UserId = userId;
                profile.BusinessId = businessId;
                //profile.Role = _roleService.GetById(roleId);

                // create userProfile
                var userProfileId = _userProfileService.Create(profile).Id;

                contactListModel.UserProfileId = userProfileId;
                var contactList = _contactListService.Create(contactListModel);

                var contacts = await parser.ParseFile(contactList.FileUrl);

                contacts.ForEach(x => _contactService.Create(new Contact
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ContactListId = contactList.Id,
                    Email = x.Email,
                    Phone = x.Phone,
                    Subscribed = x.Subscribed
                }));

                campaignModel.UserProfileId = userProfileId;
                var campaign = _campaignService.Create(campaignModel);

                _campaignService.Add(new CampaignContactList
                {
                    Campaign = campaign,
                    CampaignId = campaign.Id,
                    ContactList = contactList,
                    ContactListId = contactList.Id
                });

                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpGet("settings/data")]
        public IActionResult GetSettingsData()
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

            var business = _businessService.GetById(userProfile.BusinessId);

            return Ok(new
            {
                user = new UserProfileModel
                {
                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    Phone = userProfile.Phone,
                    Id = userProfile.Id,
                    PictureUrl = userProfile.PictureUrl,
                    BusinessRole = userProfile.BusinessRole,
                    Role = "",
                    Scope = new List<string>()
                },
                business = new BusinessModel
                {
                    Id = business.Id,
                    Email = business.Email,
                    Address = business.Address,
                    Category = business.Category,
                    Membership = business.Membership,
                    Name = business.Name,
                    Website = business.Website
                }
            });
        }

        [HttpPut("updateBusiness")]
        public IActionResult UpdateBusiness([FromBody] BusinessModel model)
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

            var currentUserProfile = _userProfileService.GetAllByUserId(userId)
                .FirstOrDefault(x => x.Default && x.Deleted == 0);

            if (currentUserProfile == null || currentUserProfile.Id == 0)
                return NotFound(
                    new
                    {
                        message = "UserProfile Not Found"
                    });

            //check the user has access to the business

            // map model to entity and set id
            var business = _mapper.Map<Business>(model);

            try
            {
                // update user 
                _businessService.Update(business);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }


        [HttpGet]
        public IActionResult GetUserProfile()
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

            var user = _userService.GetById(userId);

            if (userProfile == null || userProfile.Id == 0)
            {
                try
                {
                    var business = _businessService.Create(new Business()
                    {
                        Address = "",
                        Category = "",
                        Deleted = 0,
                        Membership = "free",
                        Name = user.BusinessName,
                        Website = "",
                        Email = ""
                    });
                    var up = _userProfileService.Create(new UserProfile()
                    {
                        BusinessRole = "",
                        BusinessId = business.Id,
                        Deleted = 0,
                        Default = true,
                        UserId = userId,
                        FirstName = user.FullName.Trim().Split(' ')[0],
                        LastName = user.FullName.Trim().Split(' ')[1],
                        Phone = "",
                        PictureUrl = ""
                    });

                    return Ok(up);
                }
                catch (Exception e)
                {
                    //log error
                    return NotFound(
                        new
                        {
                            message = "UserProfile Not Found"
                        });
                }
                
            }
                

            var response = new UserProfileModel
            {
                Id = userProfile.Id,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Phone = userProfile.Phone,
                PictureUrl = userProfile.PictureUrl,
                BusinessRole = userProfile.BusinessRole,
                Role = userProfile.Role?.Name,
                Scope = userProfile.Role?.Scope?.Split(',').ToList()
            };

            return Ok(response);
        }

        [HttpPut("updateProfile")]
        public IActionResult UpdateProfile([FromBody] UserProfileUpdateModel model)
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

            var currentUserProfile = _userProfileService.GetAllByUserId(userId)
                .FirstOrDefault(x => x.Default && x.Deleted == 0);

            if (currentUserProfile == null || currentUserProfile.Id == 0)
                return NotFound(
                    new
                    {
                        message = "UserProfile Not Found"
                    });

            // map model to entity and set id
            var profile = _mapper.Map<UserProfile>(model);

            profile.UserId = userId;
            profile.Id = currentUserProfile.Id;
            try
            {
                // update user 
                _userProfileService.Update(profile);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }


        [HttpDelete]
        public IActionResult Delete(int id)
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

            _userService.Delete(userProfile.Id);
            return Ok();
        }
    }
}