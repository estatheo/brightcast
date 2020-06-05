using System;

namespace brightcast.Entities
{
  public class CampaignSentStats
    {
        public int Id { get; set; }
        public int Delivered { get; set; }
        public int Read { get; set; }
        public int NewSubscriber { get; set; }
        public int Unsubscribed { get; set; }
        public int Replies { get; set; }
        public DateTime Date { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Deleted { get; set; }


        public int? CampaignSentId { get; set; }
        public CampaignSent CampaignSent { get; set; }
    }
}