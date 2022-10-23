using Newtonsoft.Json;

namespace LicenseProject.Messaging.IpPanelProvider;

public class JsonResult
{
    [JsonProperty("value")]
    public SendResult Value { get; set; }
}