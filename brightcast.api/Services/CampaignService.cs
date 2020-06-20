using System;
using System.Collections.Generic;
using System.Linq;
using brightcast.Entities;
using brightcast.Helpers;

namespace brightcast.Services
{
    public interface ICampaignService
    {
        Campaign GetById(int id);
        List<Campaign> GetByUserProfileId(int userProfileId);
        List<Campaign> GetByContactListId(int contactListId);
        Campaign Create(Campaign campaign);

        CampaignContactList Add(CampaignContactList ccl);
        List<CampaignContactList> GetCcl(int campaignId);

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
            return _context.Campaigns.Where(x => x.UserProfileId == userProfileId && x.Deleted == 0).ToList();
        }

        public List<Campaign> GetByContactListId(int contactListId)
        {
            var campaignIds = _context.CampaignContactLists.Where(x => x.ContactListId == contactListId).Select(x => x.CampaignId).ToList();

            var result = new List<Campaign>();

            foreach (var campaignId in campaignIds)
            {
                result.Add(_context.Campaigns.Find(campaignId));
            }

            return result;
        }

        public Campaign Create(Campaign campaign)
        {

            campaign.CreatedAt = DateTime.UtcNow;
            campaign.CreatedBy = "API";

            campaign.Deleted = 0;

            _context.Campaigns.Add(campaign);
            _context.SaveChanges();

            return campaign;
        }

        public CampaignContactList Add(CampaignContactList ccl)
        {
            _context.CampaignContactLists.Add(ccl);
            _context.SaveChanges();

            return ccl;
        }

        public List<CampaignContactList> GetCcl(int campaignId)
        {
            return _context.CampaignContactLists.Where(x => x.CampaignId == campaignId).ToList();
        }

        public void Update(Campaign campaignParam)
        {
            var campaign = _context.Campaigns.Find(campaignParam.Id);

            if (campaign == null || campaign.Deleted == 1)
                throw new AppException("Campaign not found");

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