using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using brightcast.Entities;
using brightcast.Helpers;
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
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IUserProfileService _userProfileService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IUserProfileService userProfileService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);
            
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            //var userProfile = _userProfileService.GetAllByUserId(user.Id).FirstOrDefault(x => x.Default);

            //if (userProfile == null || userProfile.Id == 0)
            //{
            //    return NotFound(
            //        new
            //        {
            //            message = "No UserProfile Found"
            //        });
            //}

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                //FirstName = userProfile.FirstName,
                //LastName = userProfile.LastName,
                //PictureUrl = userProfile.PictureUrl,
                //Phone = userProfile.Phone,
                Username = user.Username,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("registerProfile")]
        public IActionResult RegisterProfile([FromBody] RegisterProfileModel model)
        {
            //map model to entity
            var profile = _mapper.Map<UserProfile>(model);

            try
            {
                // create userProfile
                _userProfileService.Create(profile);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var users = _userService.GetAll();
        //    var model = _mapper.Map<IList<UserModel>>(users);
        //    return Ok(model);
        //}

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var userProfile = _userProfileService.GetAllByUserId(user.Id).FirstOrDefault(x => x.Default);

            if (userProfile == null || userProfile.Id == 0)
            {
                return NotFound(
                    new
                    {
                        message = "No UserProfile Found"
                    });
            }


            return Ok(userProfile);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            try
            {
                // update user 
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/UpdateProfile")]
        public IActionResult UpdateProfile(int id, [FromBody]UpdateModel model)
        {
            // map model to entity and set id
            var profile = _mapper.Map<UserProfile>(model);
            var userProfile = _userProfileService.GetAllByUserId(id).FirstOrDefault(x => x.Default);

            if (userProfile == null || userProfile.Id == 0)
            {
                return Ok(
                    new
                    {
                        Error = "No UserProfile Found"
                    });
            }
            profile.UserId = id;
            profile.Id = userProfile.Id;
            profile.BusinessId = userProfile.BusinessId;
            profile.Default = true;

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



        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}
