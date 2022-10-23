using System.ComponentModel.DataAnnotations;

namespace LicenseProject.ApiModels.Clients;

public class ValidateClientModel
{
    [Required(ErrorMessage = "سریال لایسنس الزامیست")]
    public string?AppSerial { get; set; }
    public string?SystemSerial { get; set; }
}