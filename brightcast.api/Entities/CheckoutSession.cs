using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brightcast.Entities
{
    public class CheckoutSession
    {
        public int Id { get; set; }
        public string StripeCheckoutSessionId { get; set; }
        public string PaymentStatus { get; set; }
        public string StripeCustomerId { get; set; }
        public string StripeSubscriptionId { get; set; }
        public string Mode { get; set; }
        public int UserProfileId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Deleted { get; set; }
        
        public UserProfile UserProfile { get; set; }
    }
}
