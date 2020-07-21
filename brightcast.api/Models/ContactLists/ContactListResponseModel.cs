namespace brightcast.Models.ContactLists
{
  public class ContactListResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Contacts { get; set; }
        public int Campaigns { get; set; }
        public int Unsubscribed { get; set; }
        public string KeyString { get; set; }
    }
}