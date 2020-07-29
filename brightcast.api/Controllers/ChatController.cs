using System.Linq;
using AutoMapper;
using brightcast.Entities;
using brightcast.Helpers;
using brightcast.Models.Chats;
using brightcast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace brightcast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;

        public ChatController(
            IChatService chatService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _chatService = chatService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpGet("ofList/{id}/")]
        public IActionResult GetAllByContactListId(int id)
        {
            var chatMessages = _chatService.GetAllByContactListId(id);
            return Ok(chatMessages.Select(x => new ChatModel
            {
                Id = x.Id,
                SenderId = x.SenderId,
                SenderName = x.SenderName,
                AvatarUrl = x.AvatarUrl,
                Type = x.Type,
                Files = x.Files,
                Text = x.Text,
                CreatedAt = x.CreatedAt,
                ContactListId = x.ContactListId,
                CampaignId = x.CampaignId,
            }));
        }

        [HttpGet("ofCampaign/{id}/")]
        public IActionResult GetAllByCampaignId(int id)
        {
            var chatMessages = _chatService.GetAllByCampaignId(id);
            return Ok(chatMessages.Select(x => new ChatModel
            {
                Id = x.Id,
                SenderId = x.SenderId,
                SenderName = x.SenderName,
                AvatarUrl = x.AvatarUrl,
                Type = x.Type,
                Files = x.Files,
                Text = x.Text,
                CreatedAt = x.CreatedAt,
                ContactListId = x.ContactListId,
                CampaignId = x.CampaignId,
            }));
        }

        [HttpPost("new")]
        public IActionResult Create([FromBody] ChatModel model)
        {
            try
            {
                _chatService.Create(_mapper.Map<ChatMessage>(model));

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
            _chatService.Delete(id);
            return Ok();
        }
    }
}