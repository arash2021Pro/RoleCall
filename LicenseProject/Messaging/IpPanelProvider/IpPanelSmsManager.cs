namespace LicenseProject.Messaging.IpPanelProvider;

public class IpPanelSmsManager:SmsManager
{
    private IpPanelService _smsService = null;
    private IpPanelOptions _options;

    public IpPanelSmsManager(IpPanelOptions options)
    {
        _smsService=new IpPanelService(options.Token);
        _options = options;
    }
    public async override Task<string> SendOtpMessage(string template, string number, List<string> parameters)
    {
        var dic = new Dictionary<string, string>();
        var index = 0;
        foreach (var parameter in parameters)
        {
            index++;
            dic.Add("param" + index,parameter);
        }

        var result =await _smsService.SendSmsByPatternAsync(template, _options.LineNo, new string[] {number}, dic);

        return result.Data.BulkId.ToString();
    }

    public async override Task<string> SendMessage(string number, string message)
    {
        var result =await _smsService.SendSmsAsync(_options.LineNo, new string[] {number}, message);

        return result.Data.BulkId.ToString();
    }

    public async override Task<string[]> SendMessages(string[] number, string message)
    {
        var result =await _smsService.SendSmsAsync(_options.LineNo, number, message);

        return new[] {result.Data.BulkId.ToString()};
    }

    public async override Task<string[]> SendOtpMessages(string template, string[] numbers, List<string> parameters)
    {
        var dic = new Dictionary<string, string>();
        var index = 0;
        foreach (var parameter in parameters)
        {
            index++;
            dic.Add("param" + index,parameter);
        }

        var result =await _smsService.SendSmsByPatternAsync(template, _options.LineNo, numbers, dic);

        return new[] {result.Data.BulkId.ToString()};
    }
}