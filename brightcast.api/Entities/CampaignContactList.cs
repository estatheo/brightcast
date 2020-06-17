using System;
using System.Collections.Generic;

namespace brightcast.Entities
{
  public class CampaignContactList
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }

        public int ContactListId { get; set; }
        public ContactList ContactList { get; set; }

    }
}