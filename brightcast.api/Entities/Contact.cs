using System;
using System.Collections.Generic;

namespace brightcast.Entities
{
  public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Subscribed { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Deleted { get; set; }

        public int ContactListId { get; set; }
        public ContactList ContactList { get; set; }

        public ICollection<TemplateMessage> TemplateMessages { get; set; }
        public ICollection<CampaignMessage> CampaignMessages { get; set; }
        public ICollection<ReceiveMessage> ReceiveMessages { get; set; }
    }
}