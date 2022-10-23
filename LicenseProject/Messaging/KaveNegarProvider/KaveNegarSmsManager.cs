using Kavenegar;
using Kavenegar.Core.Exceptions;
using Kavenegar.Core.Models;

namespace LicenseProject.Messaging.KaveNegarProvider;

public class KaveNegarSmsManager:SmsManager
{
    private KaveNegarOptions _options;
    private KavenegarApi smsApi;
    
    public KaveNegarSmsManager(KaveNegarOptions options)
    {
        _options = options;
        smsApi = new KavenegarApi(options.Token);
    }
    public  override async Task<string> SendOtpMessage(string template, string number, List<string> parameters)
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

    public  override async Task<string> SendMessage(string number, string message)
    {
        if (number.StartsWith("98"))
        {
            number = "0" + number.Substring(2);
        }

        SendResult result = null;

        try
        {
            result = await smsApi.Send(_options.LineNo, number, message);
        }
        catch (ApiException ex) {
            // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
            Console.Write("Message : " + ex.Message);
        } catch (HttpException ex) {
            // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
            Console.Write("Message : " + ex.Message);
        }

        if (result !=null && result.Status == 200)
        {
            return result.Messageid.ToString();
        }

        return null;
    }

    public  override async Task<string[]> SendMessages(string[] numbers, string message)
    {
        var receptors = numbers
            .Where(x => x.StartsWith("98"))
            .Select(x => x = "0" + x.Substring(2))
            .ToList();

        var senders = numbers
            .Select(z => _options.LineNo)
            .ToList();

        var messages = numbers
            .Select(x => message)
            .ToList();
        List<SendResult> result = null;

        try
        {
            result = await smsApi.SendArray(senders, receptors, messages);
        }
        catch (ApiException ex) {
            // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
            Console.Write("Message : " + ex.Message);
        } catch (HttpException ex) {
            // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
            Console.Write("Message : " + ex.Message);
        }

        if (result != null)
        {
            return result
                .Select(x => x.Messageid.ToString())
                .ToArray();
        }

        return null;
    }

    public  override async Task<string[]> SendOtpMessages(string template, string[] numbers, List<string> parameters)
    {
        var smsTemplate=_options.SmsTemplates.FirstOrDefault(x => x.Name == template);

        SendResult sendResult = null;
        try
        {
            sendResult = await smsApi.VerifyLookup(numbers[0], parameters[0], parameters[1], parameters[2], template);

            if (sendResult != null && sendResult.Status == 20)
                return new []{sendResult.Messageid.ToString()};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }

        return null;
    }
}