using Newtonsoft.Json;

namespace LicenseProject.Messaging.IpPanelProvider;

public class SendResult
{
    [JsonProperty("status")]
    public string Status { get; set; }
    [JsonProperty("code")]
    public int Code { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
    
    [JsonProperty("data")]
    public BulkResult Data { get; set; }
}