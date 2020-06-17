using System;
using System.Collections.Generic;

namespace brightcast.Entities
{
  public class ContactList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Deleted { get; set; }

        public List<Contact> Contacts { get; } = new List<Contact>();

        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        public ICollection<CampaignSent> CampaignSents { get; set; }

        public ICollection<CampaignContactList> CampaignContactLists { get; set; }
    }
}