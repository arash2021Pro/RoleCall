namespace LicenseProject.Messaging.Exceptions
{
    public class SmsParametersLostException:Exception
    {
        public SmsParametersLostException():base("The Provider must no be null or zero parameters")
        {
            
        }

        public SmsParametersLostException(string provider):base($"The {provider} Provider must no be null or zero parameters")
        {
            
        }
        
    }
}
