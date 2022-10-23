using System.ComponentModel.DataAnnotations;

namespace LicenseProject.Models;

public class AuthorizeUserViewModel
{
    
    [Required(ErrorMessage = "کد تایید الزامیست")]
    public string?Code { get; set; }
}