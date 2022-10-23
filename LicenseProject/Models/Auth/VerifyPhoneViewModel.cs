using System.ComponentModel.DataAnnotations;

namespace LicenseProject.Models.Auth;

public class VerifyPhoneViewModel
{
    
    public string? PhoneNumber { get; set; }
    [Required(ErrorMessage = "کد فعال سازی الزامیست")]
    public string? Code { get; set; }
}