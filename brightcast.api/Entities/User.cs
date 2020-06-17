using System;
using System.Collections.Generic;

namespace brightcast.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Deleted { get; set; }

        public ICollection<UserActivation> UserActivation { get; set; }
        public ICollection<ResetPassword> ResetPassword { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }

    }
}