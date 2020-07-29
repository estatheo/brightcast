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
        List<ChatMessage> GetAllByContactListId(int contactListId);
        List<ChatMessage> GetAllByCampaignId(int campaignId);
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

        public List<ChatMessage> GetAllByContactListId(int contactListId)
        {

            return _context.ChatMessages.Where(x => x.ContactListId == contactListId && x.Status == 1).ToList();
        }
        public List<ChatMessage> GetAllByCampaignId(int campaignId)
        {

            return _context.ChatMessages.Where(x => x.CampaignId == campaignId && x.Status == 1).ToList();
        }

        public ChatMessage Create(ChatMessage chatMessage)
        {
            chatMessage.Status = 1;
            _context.ChatMessages.Add(chatMessage);
            _context.SaveChanges();

            return chatMessage;
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