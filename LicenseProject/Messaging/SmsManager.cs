namespace LicenseProject.Messaging
{
    public abstract class SmsManager
    {

        public abstract Task<string> SendOtpMessage(string template, string number, List<string> parameters);
        public abstract Task<string> SendMessage(string number,string message);
        public abstract Task<string[]> SendMessages(string[] number,string message);
        public abstract Task<string[]> SendOtpMessages(string template, string[] numbers, List<string> parameters);

        
    }
}
