using RestSharp;

namespace LicenseProject.Messaging.IpPanelProvider;

public class IpPanelService
{
    private string _apiKey;
    private string baseUrl = "http://rest.ippanel.com/v1";


    public IpPanelService(string apiKey)
    {
        _apiKey = apiKey;
    }
    private RestClient _restClient;

    public RestClient GetClient()
    {
        if (_restClient == null)
        {
            _restClient = new RestClient(baseUrl);
        }

        return _restClient;
    }

    private RestRequest MakePostRequest(string path)
    {
        var request = new RestRequest(path, Method.POST);
        request.AddHeader("Authorization", "AccessKey " + _apiKey);

        return request;
    }

    public async Task<SendResult> SendSmsAsync(string originNo, string[] mobileNo, string message)
    {
        var client = GetClient();
        var request = MakePostRequest("/messages");
        
        request.AddParameter("originator", originNo);
        request.AddParameter("recipients", mobileNo);
        request.AddParameter("message", message);

        var response=await client.ExecuteAsync<JsonResult>(request);

        if (response.IsSuccessful)
            return response.Data.Value;
        return null;

    }
    
    public  async Task<SendResult> SendSmsByPatternAsync(string patternCode,string originNo, string[] mobileNo,Dictionary<string,string> patterns)
    {
        var client = GetClient();
        var request = MakePostRequest("/messages/patterns/send");
        
        request.AddParameter("pattern_code", patternCode);
        request.AddParameter("originator", originNo);
        request.AddParameter("recipients", mobileNo);
        request.AddParameter("values", patterns);

        var response=await client.ExecuteAsync<JsonResult>(request);

        if (response.IsSuccessful)
            return response.Data.Value;
        return null;

    }

}