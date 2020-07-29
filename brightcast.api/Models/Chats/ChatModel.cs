using System;
namespace brightcast.Models.Chats
{
  public class ChatModel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string AvatarUrl { get; set; }
        public string Type { get; set; }
        public Boolean Reply { get; set; }
        public object Files { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CampaignId { get; set; }
        public int ContactListId { get; set; }
        public int Status { get; set; }
    }
}