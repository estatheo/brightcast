using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brightcast.Models.Campaigns
{
    public class CampaignDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string FileUrl { get; set; }
        public int Status { get; set; }

        public int Read { get; set; }
        public int ReadPercentage { get; set; }
        public int Replies { get; set; }
        public int RepliesPercentage { get; set; }
        public int Subscribed { get; set; }
        public int SubscribedPercentage { get; set; }
        public int Delivered { get; set; }
        public int DeliveredPercentage { get; set; }

    }
}
