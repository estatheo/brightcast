using System;
using System.Collections.Generic;

namespace brightcast.Entities
{
  public class Campaign
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string FileUrl { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Deleted { get; set; }

        public int? UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        public ICollection<TemplateMessage> TemplateMessages { get; set; }
        public ICollection<CampaignMessage> CampaignMessages { get; set; }
        public ICollection<ReceiveMessage> ReceiveMessages { get; set; }

        public ICollection<CampaignSent> CampaignSents { get; set; }

        public ICollection<CampaignContactList> CampaignContactLists { get; set; }

    }
}