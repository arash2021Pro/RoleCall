namespace LicenseProject.Messaging
{
    public class SmsOptions
    {
        public string? Token { get; set; }

        public string? LineNo { get; set; }

        public List<SmsTemplate> SmsTemplates { get; set; }
    }
}
