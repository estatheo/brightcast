using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brightcast.Models.Twilio
{
    public class TwilioWhatsappCallbackModel
    {
        public int Id { get; set; }
        public string MessageSid { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime DateUpdated { get; set; }
        public string ErrorCode { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string MessageStatus { get; set; }
        public string ErrorMessage { get; set; }
    }
}
