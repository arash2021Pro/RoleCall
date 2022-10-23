using Ghasedak.Core;
using Ghasedak.Core.Models;
using LicenseProject.Messaging.Exceptions;
using Results = Ghasedak.Core.Models.Results;

namespace LicenseProject.Messaging.GhasedakProvider
{
    public class GhasedakSmsManager:SmsManager
    {

        private const int OTP_TEXT_MESSAGE = 1;
        private const int OTP_VOICE_MESSAGE = 2;
        
        private Api smsApi = null;
        private GhasedakOptions _options;

        public GhasedakSmsManager(GhasedakOptions options)
        {
            smsApi=new Api(options.Token);
            _options = options;
        }
        public override async Task<string> SendOtpMessage(string template, string number,List<string> parameters)
        {
            if (number.StartsWith("98"))
            {
                number = "0" + number.Substring(2);
            }
            var result = await SendOtpMessages(template, new[] {number}, parameters);
            if (result != null && result.Length > 0)
                return result[0];
            return null;
        }

        public override async Task<string> SendMessage(string number, string message)
        {
            if (number.StartsWith("98"))
            {
                number = "0" + number.Substring(2);
            }

            Results.SendResult result = null;

            try
            {
                result= await smsApi.SendSMSAsync(message, new string[] { number }, _options.LineNo, DateTime.Now, null,
                    null);
            }
            catch (Exception e)
            {
                return null;
            }
            
            if (result !=null && result.Result.Code == 200)
            {
                if (result.Items.Count > 0)
                    return result.Items[0].ToString();
            }

            return null;
        }

        public override async Task<string[]> SendMessages(string[] numbers, string message)
        {
            numbers = numbers
                .Where(x => x.StartsWith("98"))
                .Select(x => x = "0" + x.Substring(2))
                .ToArray();

            var result= await smsApi.SendSMSAsync(message, numbers, _options.LineNo, DateTime.Now, null,
                null);
            
            if (result !=null && result.Result.Code == 200)
            {
                if (result.Items.Count > 0)
                    return result.Items.Select(x => x.ToString()).ToArray();
            }

            return null;
        }

        public override async Task<string[]> SendOtpMessages(string template, string[] numbers, List<string> parameters)
        {
            var smsTemplate=_options.SmsTemplates.FirstOrDefault(x => x.Name == template);

            if (smsTemplate == null)
                return null;
            
            if (parameters == null || parameters.Count == 0)
                throw new SmsParametersLostException(nameof(GhasedakSmsManager));

            Results.SendResult result = null;
            try
            {
                result = await smsApi.VerifyAsync(OTP_TEXT_MESSAGE, smsTemplate.Value, numbers,
                    parameters.Count >= 1 ? parameters[0] : null,
                    parameters.Count >= 2 ? parameters[1] : null,
                    parameters.Count >= 3 ? parameters[2] : null,
                    parameters.Count >= 4 ? parameters[3] : null,
                    parameters.Count >= 5 ? parameters[4] : null,
                    parameters.Count >= 6 ? parameters[5] : null,
                    parameters.Count >= 7 ? parameters[6] : null,
                    parameters.Count >= 8 ? parameters[7] : null,
                    parameters.Count >= 9 ? parameters[8] : null,
                    parameters.Count >= 10 ? parameters[9] : null
                );
            }
            catch (Exception e)
            {
                return null;
            }
            

            if (result !=null && result.Result.Code == 200)
            {
                return result.Items.Select(x => x.ToString()).ToArray();
            }

            return null;
        }

         
    }
}
