using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;

namespace LicenseProject.Models;

public class AdminViewModel
{
    [Required(ErrorMessage = "نام کاربری الزامیست")]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "کلمه عبور الزامیست")]
    public string? Password { get; set; }
}