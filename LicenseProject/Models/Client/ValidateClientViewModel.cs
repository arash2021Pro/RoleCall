using System.ComponentModel.DataAnnotations;

namespace LicenseProject.Models.Client;

public class ValidateClientViewModel
{
    [Required(ErrorMessage = "سریال لایسنس الزامیست")]
    public string?AppSerial { get; set; }
    public string?SystemSerial { get; set; }
}