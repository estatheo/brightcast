using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brightcast.Models.Contacts
{
    public class ContactParseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Subscribed { get; set; }
    }
}
