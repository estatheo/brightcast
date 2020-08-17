﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using brightcast.Entities;
using brightcast.Helpers;
using brightcast.Models.Chats;
using brightcast.Models.Twilio;
using brightcast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace brightcast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IChatService _chatService;
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public ChatController(
            IChatService chatService,
            IContactService contactService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _chatService = chatService;
            _contactService = contactService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }


        [HttpGet("ofCampaignAndContact/{campaignId}/{contactId}")]
        public IActionResult GetAllByCampaignIdAndContactId(int campaignId, int contactId)
        {
            var chatMessages = _chatService.GetAllByCampaignAndContactId(campaignId, contactId);
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
                ContactId = x.ContactId,
                CampaignId = x.CampaignId,
            }));
        }

        [HttpPost("new")]
        public async Task<IActionResult> Create([FromBody] ChatModel model)
        {
            try
            {
                var contact = _contactService.GetById(model.ContactId);

                var client = new HttpClient();

                var requestModel = new FormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("From", $"{_appSettings.TwilioWhatsappNumber}"),
                        new KeyValuePair<string, string>("Body", $"{model.Text}"),
                        //new KeyValuePair<string, string>("StatusCallback",
                        //    $"{_appSettings.ApiBaseUrl}/message/callback/template"),
                        new KeyValuePair<string, string>("To", $"whatsapp:{contact.Phone}")
                    }
                );
                var req = new HttpRequestMessage(HttpMethod.Post,
                        $"https://api.twilio.com/2010-04-01/Accounts/{_appSettings.TwilioAccountSID}/Messages.json")
                    { Content = requestModel };

                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        $"{_appSettings.TwilioAccountSID}:{_appSettings.TwilioAuthToken}")));

                var result = await client.SendAsync(req);

                var resultModel =
                    JsonConvert.DeserializeObject<TwilioTemplateMessageModel>(
                        await result.Content.ReadAsStringAsync());
                
                _chatService.Create(_mapper.Map<ChatMessage>(model));

                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPost("sendInvitation")]
        public async Task<IActionResult> SendInvitation([FromBody] InvitationModel model)
        {
            try
            {
                var client = new HttpClient();

                var requestModel = new FormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>
                    {
                            new KeyValuePair<string, string>("From", $"{_appSettings.TwilioWhatsappNumber}"),
                            new KeyValuePair<string, string>("Body", $"{model.BodyMessage}"),
                            new KeyValuePair<string, string>("StatusCallback",
                                $"{_appSettings.ApiBaseUrl}/message/callback/template"),
                            new KeyValuePair<string, string>("To", $"whatsapp:{model.PhoneNumber}")
                    }
                );
                var req = new HttpRequestMessage(HttpMethod.Post,
                        $"https://api.twilio.com/2010-04-01/Accounts/{_appSettings.TwilioAccountSID}/Messages.json")
                { Content = requestModel };

                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        $"{_appSettings.TwilioAccountSID}:{_appSettings.TwilioAuthToken}")));

                var result = await client.SendAsync(req);

                var resultModel =
                    JsonConvert.DeserializeObject<TwilioTemplateMessageModel>(
                        await result.Content.ReadAsStringAsync());
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex });
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