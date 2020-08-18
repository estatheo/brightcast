using System;
using System.Collections.Generic;
namespace brightcast.Models.Chats
{
  public class InvitationModel
    {
        public int InviteeId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string BodyMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CampaignId { get; set; }
        public int ContactId { get; set; }
        public string PhoneNumber { get; set; }
    }
}