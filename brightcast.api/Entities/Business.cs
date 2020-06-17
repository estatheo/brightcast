using System;
using System.Collections.Generic;

namespace brightcast.Entities
{
  public class Business
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Membership { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Deleted { get; set; }

        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}