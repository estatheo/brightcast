using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace brightcast.Models.Twilio
{
    public class TwilioWhatsappCallbackModel
    {
        public int Id { get; set; }
        public string MessageSid { get; set; }
        public string Body { get; set; }
        public string MediaContentType0 { get; set; }
        public string MediaUrl0 { get; set; }
        public int NumMedia { get; set; }
        public string SmsStatus { get; set; }

        [JsonProperty(PropertyName = "date_created")]
        public DateTime DateCreated { get; set; }

        [JsonProperty(PropertyName = "date_sent")]
        public DateTime DateSent { get; set; }

        [JsonProperty(PropertyName = "date_updated")]
        public DateTime DateUpdated { get; set; }

        [JsonProperty(PropertyName = "error_code")]
        public string ErrorCode { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        [JsonProperty(PropertyName = "message_status")]
        public string MessageStatus { get; set; }
        [JsonProperty(PropertyName = "error_message")]
        public string ErrorMessage { get; set; }
    }
}
