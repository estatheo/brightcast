using System;
using System.Collections.Generic;

namespace brightcast.Entities
{
    public class UserActivation
    {
        public int Id { get; set; }
        public Guid ActivationCode { get; set; }
        public bool Activated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Deleted { get; set; }

        public int UserId { get; set; }

    }
}