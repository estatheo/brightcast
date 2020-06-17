using System.Collections.Generic;

namespace brightcast.Models.Campaigns
{
  public class CampaignSendModel
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public string FileUrl { get; set; }
        public List<int> ContactListIds { get; set; }
        public int Id { get; set; }

    }
}