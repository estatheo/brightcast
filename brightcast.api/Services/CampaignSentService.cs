using System;
using System.Collections.Generic;
using brightcast.Entities;
using brightcast.Helpers;

namespace brightcast.Services
{
    public interface ICampaignSentService
    {
        CampaignSent GetById(int id);
        List<CampaignSent> GetAllByCampaignId(int campaignId);
        CampaignSent Create(CampaignSent campaignSent);
        void Update(CampaignSent campaignSent);
        void Delete(int id);

    }

    public class CampaignSentService : ICampaignSentService
    {
        private DataContext _context;

        public CampaignSentService(DataContext context)
        {
            _context = context;
        }

        public CampaignSent GetById(int id)
        {
            var campaignSent = _context.CampaignSents.Find(id);

            return campaignSent != null && campaignSent.Deleted == 0 ? campaignSent : null;
        }

        public List<CampaignSent> GetAllByCampaignId(int campaignId)
        {
            return _context.Campaigns.Find(campaignId).CampaignSents;
        }


        public CampaignSent Create(CampaignSent campaignSent)
        {
            // validation


            campaignSent.CreatedAt = DateTime.UtcNow;
            campaignSent.CreatedBy = "API";

            campaignSent.Deleted = 0;

            _context.CampaignSents.Add(campaignSent);
            _context.SaveChanges();

            return campaignSent;
        }

        public void Update(CampaignSent campaignSentParam)
        {
            var campaignSent = _context.CampaignSents.Find(campaignSentParam.Id);

            if (campaignSent == null || campaignSent.Deleted == 1)
                throw new AppException("CampaignSent not found");

            // update Name if it has changed
            if (campaignSentParam.Date != campaignSent.Date)
            {
                campaignSent.Date = campaignSentParam.Date;
            }


            // update user properties if provided

            campaignSent.UpdatedBy = campaignSentParam.UpdatedBy;

            campaignSent.UpdatedAt = campaignSentParam.UpdatedAt;
            
            _context.CampaignSents.Update(campaignSent);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var campaignSent = _context.CampaignSents.Find(id);
            if (campaignSent != null)
            {
                campaignSent.Deleted = 1;
                
                _context.CampaignSents.Update(campaignSent);
                _context.SaveChanges();
            }
        }

    }
}