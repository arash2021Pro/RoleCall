using Newtonsoft.Json;

namespace LicenseProject.Messaging.IpPanelProvider;

public class BulkResult
{
    [JsonProperty("bulk_id")]

    public long BulkId { get; set; }
}