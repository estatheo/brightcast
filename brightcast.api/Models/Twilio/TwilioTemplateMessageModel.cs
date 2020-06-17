using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brightcast.Models.Twilio
{
    public class TwilioTemplateMessageModel
    {
        public string Account_sid { get; set; }
        public string Sid { get; set; }
        public string Body { get; set; }
        public DateTime? Date_Created { get; set; }
        public DateTime? Date_Sent { get; set; }
        public DateTime? Date_Updated { get; set; }
        public string Error_Code { get; set; }
        public string Error_Message { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Status { get; set; }
    }
}
