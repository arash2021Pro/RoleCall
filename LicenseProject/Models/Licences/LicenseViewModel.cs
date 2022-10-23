using System.ComponentModel.DataAnnotations;
using CoreBussiness.BussinessEntity.Serials;

namespace LicenseProject.Models.Licences;

public class LicenseViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "نوع افزار الزامیست")]
    public SoftwareType SoftwareType { get; set; }
    public bool IsSmsPanelActive { get; set; }
    public bool IsMobileVersionActive { get; set; }
    [Required(ErrorMessage = "تعداد کلاینت الزامیست")]
    public int ClientCount { get; set; }
    [Required(ErrorMessage = "تعداد سریال نرم افزار الزامیست ")]
    public int AppSerialCount { get; set; }
}