using System;
using System.Collections.Generic;

namespace brightcast.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public string Phone { get; set; }
        public bool Default { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Deleted { get; set; }


        public int? UserId { get; set; }
        public User User { get; set; }

        public int? BusinessId { get; set; }
        public Business Business { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public List<ContactList> ContactLists { get; set; }

        public List<Campaign> Campaigns { get; set; }
    }
}