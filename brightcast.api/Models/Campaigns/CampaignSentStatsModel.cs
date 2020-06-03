using System;

namespace brightcast.Models.Campaigns
{
  public class CampaignSentStatsModel
    {
        public int Id { get; set; }
        public int Delivered { get; set; }
        public int Read { get; set; }
        public int NewSubscriber { get; set; }
        public int Unsubscribed { get; set; }
        public int Replies { get; set; }
        public DateTime Date { get; set; }


        public int CampaignSentId { get; set; }
    }
}