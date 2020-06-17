using System;
using System.Collections.Generic;

namespace brightcast.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Scope { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Deleted { get; set; }

        public UserProfile UserProfiles { get; set; }
        public int UserProfileId { get; set; }
    }
}