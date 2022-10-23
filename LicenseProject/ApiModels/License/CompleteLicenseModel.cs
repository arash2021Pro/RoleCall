using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;

namespace LicenseProject.ApiModels.License;

public class CompleteLicenseModel
{
    public string? LicenseCode { get; set; }
    [Required(ErrorMessage = "نام الزامیست")]
    [ShouldContainOnlyPersianLetters(AllowWhitespace = true,ErrorMessage = "کارکتر های فارسیی مجاز هستند")]
    public string?Name { get; set; }
    [Required(ErrorMessage = "نام خانوادگی الزامیست")]
    [ShouldContainOnlyPersianLetters(AllowWhitespace = true,ErrorMessage = "کارکتر های فارسیی مجاز هستند")]
    public string?LastName { get; set; }
    [Required(ErrorMessage = "شماره همراه الزامیست")]
    public string?PhoneNumber { get; set; }
    [Required(ErrorMessage = "شماره ملی یا شناسه شرکت الزامیست")]
    public string? LegalCode { get; set; }
    [MaxLength(11,ErrorMessage = "حداکثر کارکتر هایازده می باشد"),MinLength(8,ErrorMessage = "حداقل کارکتر هشت می باشد")]
    [Required(ErrorMessage = "شماره ثابت الزامیست")]
    public string?ConstPhone { get; set; }
    [Required(ErrorMessage = "نام شرکت الزامیست")]
    [ShouldContainOnlyPersianLetters(AllowWhitespace = true,ErrorMessage = "کارکتر های فارسیی مجاز هستند")]
    public string?CompanyName { get; set; }
    [ShouldContainOnlyPersianLetters(AllowWhitespace = true,ErrorMessage = "کارکتر های فارسیی مجاز هستند")]
    [Required(ErrorMessage = "آدرس شرکت الزامیست")]
    public string? CompanyAddress { get; set; }
}