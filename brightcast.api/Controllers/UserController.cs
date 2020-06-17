using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserAuthenticateModel model)
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
            
            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserRegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                var result = _userService.Create(user, model.Password);
                await _userService.SendConfirmationEmail(result.Id, result.Username);

                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("verify/{code}")]
        public IActionResult Verify(Guid code)
        {
            _userService.ActivateUser(code);

            return Ok();
        }


        [AllowAnonymous]
        [HttpPost("resetPassword/confirm")]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            _userService.ResetPassword(model.Code, model.Password);

            return Ok();
        }


        [AllowAnonymous]
        [HttpPost("resetPassword")]
        public IActionResult RequestResetPassword([FromBody] UserUpdateModel model)
        {
            var code = _userService.RequestResetPassword(model.Username);

            if (code == null)
            {
                return NotFound("User not found");
            }

            _userService.SendResetPasswordEmail(code ?? Guid.Empty, model.Username);

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UserUpdateModel model)
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

        //[HttpDelete("{id}")]
        //public IActionResult DeleteById(int id)
        //{
        //    //check permissions
        //    _userService.Delete(id);
        //    return Ok();
        //}

        [HttpDelete]
        public IActionResult Delete()
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

            _userService.Delete(userId);
            return Ok();
        }
    }
}
