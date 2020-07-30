using System;
using System.Collections.Generic;
using System.Linq;
using brightcast.Entities;
using brightcast.Helpers;

namespace brightcast.Services
{
    public interface IMessageService
    {
        TemplateMessage AddTemplateMessage(TemplateMessage entity);
        TemplateMessage UpdateTemplateMessage(TemplateMessage entity);
        TemplateMessage GetTemplateMessageByMessageId(string messageId);
        TemplateMessage GetLastByTo(string to);
        ReceiveMessage AddReceiveMessage(ReceiveMessage entity);
        ReceiveMessage GetReceiveMessageByMessageId(string messageId);
        CampaignMessage AddCampaignMessage(CampaignMessage entity);
        CampaignMessage GetCampaignMessageByMessageId(string messageId);
        List<CampaignMessage> GetCampaignMessagesByCampaignId(int campaignId);
        List<ReceiveMessage> GetReceiveMessagesByCampaignId(int campaignId);
        CampaignMessage UpdateCampaignMessage(CampaignMessage entity);
    }

    public class MessageService : IMessageService
    {
        private DataContext _context;

        public MessageService(DataContext context)
        {
            _context = context;
        }


        public TemplateMessage AddTemplateMessage(TemplateMessage entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = "API";
            entity.Deleted = 0;

            _context.TemplateMessages.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public TemplateMessage UpdateTemplateMessage(TemplateMessage entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = "API";

            _context.TemplateMessages.Update(entity);
            _context.SaveChanges();

            return entity;
        }

        public TemplateMessage GetTemplateMessageByMessageId(string messageId)
        {
            var result = _context.TemplateMessages.FirstOrDefault(x => x.MessageSid == messageId && x.Deleted == 0);

            return result;
        }

        public ReceiveMessage AddReceiveMessage(ReceiveMessage entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = "API";
            entity.Deleted = 0;

            _context.ReceiveMessages.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public ReceiveMessage GetReceiveMessageByMessageId(string messageId)
        {
            var result = _context.ReceiveMessages.FirstOrDefault(x => x.MessageSid == messageId && x.Deleted == 0);

            return result;
        }

        public CampaignMessage AddCampaignMessage(CampaignMessage entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = "API";
            entity.Deleted = 0;

            _context.CampaignMessages.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public CampaignMessage GetCampaignMessageByMessageId(string messageId)
        {
            var result = _context.CampaignMessages.FirstOrDefault(x => x.MessageSid == messageId && x.Deleted == 0);

            return result;
        }

        public List<CampaignMessage> GetCampaignMessagesByCampaignId(int campaignId)
        {
            return _context.CampaignMessages.Where(x => x.CampaignId == campaignId && x.Deleted == 0).ToList();
        }

        public List<ReceiveMessage> GetReceiveMessagesByCampaignId(int campaignId)
        {
            return _context.ReceiveMessages.Where(x => x.CampaignId == campaignId && x.Deleted == 0).ToList();
        }

        public CampaignMessage UpdateCampaignMessage(CampaignMessage entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = "API";

            _context.CampaignMessages.Update(entity);
            _context.SaveChanges();

            return entity;
        }
        
        public TemplateMessage GetLastByTo(string to)
        {
            return _context.TemplateMessages.Where(x => x.To == to && x.Deleted == 0).OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();
        }
    }
}