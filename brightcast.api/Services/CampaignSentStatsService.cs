using System;
using System.Collections.Generic;
using System.Linq;
using brightcast.Entities;
using brightcast.Helpers;

namespace brightcast.Services
{
    public interface ICampaignSentStatsService
    {
        CampaignSentStats GetById(int id);
        List<CampaignSentStats> GetByCampaignSentId(int campaignSentId);
        CampaignSentStats Create(CampaignSentStats campaignSentStats);
        void Update(CampaignSentStats campaignSentStats);
        void Delete(int id);

    }

    public class CampaignSentStatsService : ICampaignSentStatsService
    {
        private DataContext _context;

        public CampaignSentStatsService(DataContext context)
        {
            _context = context;
        }

        public CampaignSentStats GetById(int id)
        {
            var campaignSentStats = _context.CampaignSentStatses.Find(id);

            return campaignSentStats != null && campaignSentStats.Deleted == 0 ? campaignSentStats : null;
        }

        public List<CampaignSentStats> GetByCampaignSentId(int campaignSentId)
        {
            return _context.CampaignSents.Find(campaignSentId)?.CampaignSentStatses?.ToList();
        }


        public CampaignSentStats Create(CampaignSentStats campaignSentStats)
        {
            // validation


            campaignSentStats.CreatedAt = DateTime.UtcNow;
            campaignSentStats.CreatedBy = "API";

            campaignSentStats.Deleted = 0;

            _context.CampaignSentStatses.Add(campaignSentStats);
            _context.SaveChanges();

            return campaignSentStats;
        }

        public void Update(CampaignSentStats campaignSentStatsParam)
        {
            var campaignSentStats = _context.CampaignSentStatses.Find(campaignSentStatsParam.Id);

            if (campaignSentStats == null || campaignSentStats.Deleted == 1)
                throw new AppException("CampaignSentStats not found");

            // update Name if it has changed
            if ( campaignSentStatsParam.Date != campaignSentStats.Date)
            {
                campaignSentStats.Date = campaignSentStatsParam.Date;
            }

            // update Name if it has changed
            if (campaignSentStatsParam.Read != campaignSentStats.Read)
            {
                campaignSentStats.Read = campaignSentStatsParam.Read;
            }

            // update Name if it has changed
            if (campaignSentStatsParam.Delivered != campaignSentStats.Delivered)
            {
                campaignSentStats.Delivered = campaignSentStatsParam.Delivered;
            }

            // update Name if it has changed
            if (campaignSentStatsParam.NewSubscriber != campaignSentStats.NewSubscriber)
            {
                campaignSentStats.NewSubscriber = campaignSentStatsParam.NewSubscriber;
            }

            // update Name if it has changed
            if (campaignSentStatsParam.Unsubscribed != campaignSentStats.Unsubscribed)
            {
                campaignSentStats.Unsubscribed = campaignSentStatsParam.Unsubscribed;
            }

            // update Name if it has changed
            if (campaignSentStatsParam.Replies != campaignSentStats.Replies)
            {
                campaignSentStats.Replies = campaignSentStatsParam.Replies;
            }

            // update user properties if provided

            campaignSentStats.UpdatedBy = campaignSentStatsParam.UpdatedBy;

            campaignSentStats.UpdatedAt = campaignSentStatsParam.UpdatedAt;
            
            _context.CampaignSentStatses.Update(campaignSentStats);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var campaignSentStats = _context.CampaignSentStatses.Find(id);
            if (campaignSentStats != null)
            {
                campaignSentStats.Deleted = 1;
                
                _context.CampaignSentStatses.Update(campaignSentStats);
                _context.SaveChanges();
            }
        }

    }
}