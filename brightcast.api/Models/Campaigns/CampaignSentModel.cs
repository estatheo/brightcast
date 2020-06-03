using System;

namespace brightcast.Models.Campaigns
{
  public class CampaignSentModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }


        public int CampaignId { get; set; }
        public int ContactListId { get; set; }
        public int UserId { get; set; }
    }
}