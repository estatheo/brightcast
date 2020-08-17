using System;
using System.Collections.Generic;
namespace brightcast.Models.Chats
{
  public class ChatModel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string AvatarUrl { get; set; }
        public string Type { get; set; }
        public bool Reply { get; set; }
        public string Files { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CampaignId { get; set; }
        public int ContactId { get; set; }
    }
}