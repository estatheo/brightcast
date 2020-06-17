using System.Collections.Generic;
using brightcast.Entities;
using brightcast.Models.ContactLists;

namespace brightcast.Models.Campaigns
{
  public class CampaignResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string FileUrl { get; set; }
        public int Status { get; set; }
        public int Sent { get; set; }
        public int Read { get; set; }
        public int Response { get; set; }
        public List<int> ContactListIds { get; set; }

        
    }
}