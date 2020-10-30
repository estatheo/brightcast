using System;

namespace brightcast.Models.Contacts
{
  public class ContactMessageModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Body { get; set; }
        public DateTime Time { get; set; }
    }
}