namespace brightcast.Models.Contacts
{
  public class ContactModel
    {
        public int Id { get; set; }
        public string Channels { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Subscribed { get; set; }
        public int ContactListId { get; set; }
    }
}