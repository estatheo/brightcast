using System;
using System.Collections.Generic;

namespace brightcast.Entities
{
  public class CampaignSent
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }


        public int CampaignId { get; set; }
        public int ContactListId { get; set; }
        public int UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Deleted { get; set; }

        public List<CampaignSentStats> CampaignSentStatses { get; set; }

    }
}