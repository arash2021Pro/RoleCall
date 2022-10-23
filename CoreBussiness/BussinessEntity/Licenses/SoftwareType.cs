using System.ComponentModel.DataAnnotations;

namespace CoreBussiness.BussinessEntity.Serials;

public enum SoftwareType
{
    [Display(Name = "پایه")]
    Basic,
    [Display(Name = "پیشرفته")]
    Advanced,
    [Display(Name = "حرفه ای")]
    Professional
}