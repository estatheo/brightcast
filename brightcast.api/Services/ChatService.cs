using System;
using System.Collections.Generic;
using System.Linq;
using brightcast.Entities;
using brightcast.Helpers;

namespace brightcast.Services
{
    public interface IChatService
    {
        ChatMessage GetById(int id);
        List<ChatMessage> GetAllByCampaignAndContactId(int campaignId, int contactId);
        List<ChatMessage> GetByUserProfileId(int userProfileId);
        ChatMessage Create(ChatMessage chatMessage);
        void Delete(int id);

    }

    public class ChatService : IChatService
    {
        private DataContext _context;

        public ChatService(DataContext context)
        {
            _context = context;
        }

        public ChatMessage GetById(int id)
        {
            var chatMessage = _context.ChatMessages.Find(id);

            return chatMessage != null && chatMessage.Status == 0 ? chatMessage : null;
        }

        public List<ChatMessage> GetAllByCampaignAndContactId(int campaignId, int contactId)
        {

            return _context.ChatMessages.Where(x => x.CampaignId == campaignId && x.ContactId == contactId && x.Status == 1).ToList();
        }

        public List<ChatMessage> GetByUserProfileId(int userProfileId)
        {
            var campaignIds = _context.Campaigns.Where(x => x.UserProfileId == userProfileId).Select(x => x.Id).ToList();

            return _context.ChatMessages.Where(x => campaignIds.Contains(x.CampaignId)).ToList();
        }

        public ChatMessage Create(ChatMessage chatMessage)
        {
            try
            {
                chatMessage.Status = 1;
                _context.ChatMessages.Add(chatMessage);
                _context.SaveChanges();

                return chatMessage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }


        public void Delete(int id)
        {
            var chatMessage = _context.ChatMessages.Find(id);
            if (chatMessage != null)
            {
                chatMessage.Status = 0;
                
                _context.ChatMessages.Update(chatMessage);
                _context.SaveChanges();
            }
        }

    }
}