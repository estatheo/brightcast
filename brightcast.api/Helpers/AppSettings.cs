namespace brightcast.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ApiBaseUrl { get; set; }
        public string UiBaseUrl { get; set; }
        public string SendgridApiKey { get; set; }
        public string StorageConnectionString { get; set; }
        public string TwilioAccountSID { get; set; }
        public string TwilioAuthToken { get; set; }
        public string TwilioWhatsappNumber { get; set; }
        public string TwilioTemplateMessage { get; set; }
        public string TwilioInitializeMessage { get; set; }


    }
}