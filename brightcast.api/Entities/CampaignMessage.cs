using System;

namespace brightcast.Entities
{
  public class CampaignMessage
    {
        public int Id { get; set; }
        public string MessageSid { get; set; }
        public string Body { get; set; }
        public DateTime? Date_Created { get; set; }
        public DateTime? Date_Sent { get; set; }
        public DateTime? Date_Updated { get; set; }
        public string Error_Code { get; set; }
        public string Error_Message { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Deleted { get; set; }

        public int ContactId { get; set; }
        public Contact Contact { get; set; }
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
    }
}