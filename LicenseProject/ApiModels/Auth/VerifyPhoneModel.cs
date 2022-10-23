using System.ComponentModel.DataAnnotations;

namespace LicenseProject.ApiModels.Auth;

public class VerifyPhoneModel
{
   [Required(ErrorMessage = "کد فعال سازی الزامیست")]
   public string? Code { get; set; }  
}