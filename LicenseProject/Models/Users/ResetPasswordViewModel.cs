using System.ComponentModel.DataAnnotations;

namespace LicenseProject.Models;

public class ResetPasswordViewModel
{
    [Required(ErrorMessage = "انتخاب پسورد جدید الزامیست")]
    public string?Password { get; set; }
}