using System;
using System.Collections.Generic;
using brightcast.Entities;
using brightcast.Helpers;

namespace brightcast.Services
{
    public interface ICampaignService
    {
        Campaign GetById(int id);
        List<Campaign> GetByUserProfileId(int userProfileId);
        Campaign Create(Campaign campaign);
        void Update(Campaign campaign);
        void Delete(int id);

    }

    public class CampaignService : ICampaignService
    {
        private DataContext _context;

        public CampaignService(DataContext context)
        {
            _context = context;
        }

        public Campaign GetById(int id)
        {
            var campaign = _context.Campaigns.Find(id);

            return campaign != null && campaign.Deleted == 0 ? campaign : null;
        }

        public List<Campaign> GetByUserProfileId(int userProfileId)
        {
            return _context.UserProfiles.Find(userProfileId).Campaigns;
        }


        public Campaign Create(Campaign campaign)
        {
            // validation


            campaign.CreatedAt = DateTime.UtcNow;
            campaign.CreatedBy = "API";

            campaign.Deleted = 0;

            _context.Campaigns.Add(campaign);
            _context.SaveChanges();

            return campaign;
        }

        public void Update(Campaign campaignParam)
        {
            var campaign = _context.Campaigns.Find(campaignParam.Id);

            if (campaign == null || campaign.Deleted == 1)
                throw new AppException("Campaign not found");

            // update Name if it has changed
            if (!string.IsNullOrWhiteSpace(campaignParam.Name) && campaignParam.Name != campaign.Name)
            {
                campaign.Name = campaignParam.Name;
            }

            if (!string.IsNullOrWhiteSpace(campaignParam.Message) && campaignParam.Message != campaign.Message)
            {
                campaign.Message = campaignParam.Message;
            }

            if (!string.IsNullOrWhiteSpace(campaignParam.FileUrl) && campaignParam.FileUrl != campaign.FileUrl)
            {
                campaign.FileUrl = campaignParam.FileUrl;
            }

            // update user properties if provided

            campaign.UpdatedBy = campaignParam.UpdatedBy;

            campaign.UpdatedAt = campaignParam.UpdatedAt;
            
            _context.Campaigns.Update(campaign);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var campaign = _context.Campaigns.Find(id);
            if (campaign != null)
            {
                campaign.Deleted = 1;
                
                _context.Campaigns.Update(campaign);
                _context.SaveChanges();
            }
        }

    }
}