using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using brightcast.Entities;
using brightcast.Helpers;
using brightcast.Models.Businesses;
using brightcast.Models.Onboarding;
using brightcast.Models.UserProfiles;
using brightcast.Models.Users;
using brightcast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace brightcast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private IUserService _userService;
        private IUserProfileService _userProfileService;
        private IBusinessService _businessService;
        private IRoleService _roleService;
        private IContactListService _contactListService;
        private ICampaignService _campaignService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserProfileController(
            IUserService userService,
            IUserProfileService userProfileService,
            IMapper mapper,
            IRoleService roleService,
            IContactListService contactListService,
            ICampaignService campaignService,
            IBusinessService businessService,
            IOptions<AppSettings> appSettings
            )
        {
            _userService = userService;
            _userProfileService = userProfileService;
            _roleService = roleService;
            _contactListService = contactListService;
            _campaignService = campaignService;
            _businessService = businessService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
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
                var userProfile = _userProfileService.GetAllByUserId(userId).FirstOrDefault(x => x.Default && x.Deleted == 0);
                if (userProfile == null || userProfile.Id == 0)
                {
                    return Ok(
                        new
                        {
                            onboard = true
                        });
                }
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
        public IActionResult RegisterProfile([FromBody] OnboardingRegistrationModel model)
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

                campaignModel.UserProfileId = userProfileId;
                var campaign =_campaignService.Create(campaignModel);

                _campaignService.Add(new CampaignContactList()
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
                return BadRequest(new { message = ex.Message });
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
            var userProfile = _userProfileService.GetAllByUserId(userId).FirstOrDefault(x => x.Default && x.Deleted == 0);

            if (userProfile == null || userProfile.Id == 0)
            {
                return NotFound(
                    new
                    {
                        message = "UserProfile Not Found"
                    });
            }

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
            var currentUserProfile = _userProfileService.GetAllByUserId(userId).FirstOrDefault(x => x.Default && x.Deleted == 0);

            if (currentUserProfile == null || currentUserProfile.Id == 0)
            {
                return NotFound(
                    new
                    {
                        message = "UserProfile Not Found"
                    });
            }

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
                return BadRequest(new { message = ex.Message });
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
            var userProfile = _userProfileService.GetAllByUserId(userId).FirstOrDefault(x => x.Default && x.Deleted == 0);

            if (userProfile == null || userProfile.Id == 0)
            {
                return NotFound(
                    new
                    {
                        message = "UserProfile Not Found"
                    });
            }

            return Ok(userProfile);
        }

        [HttpPut("updateProfile")]
        public IActionResult UpdateProfile([FromBody]UserProfileUpdateModel model)
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
            var currentUserProfile = _userProfileService.GetAllByUserId(userId).FirstOrDefault(x => x.Default && x.Deleted == 0);

            if (currentUserProfile == null || currentUserProfile.Id == 0)
            {
                return NotFound(
                    new
                    {
                        message = "UserProfile Not Found"
                    });
            }

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
                return BadRequest(new { message = ex.Message });
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
            var userProfile = _userProfileService.GetAllByUserId(userId).FirstOrDefault(x => x.Default && x.Deleted == 0);

            if (userProfile == null || userProfile.Id == 0)
            {
                return NotFound(
                    new
                    {
                        message = "UserProfile Not Found"
                    });
            }

            _userService.Delete(userProfile.Id);
            return Ok();
        }
    }
}
