using CoreBussiness.BussinessEntity.Serials;

namespace CoreBussiness.BussinessEntity.Licenses;

public class LicenseDataViewModel
{
    public int Id { get; set; }
    public string?Name { get; set; }
    public string?LastName { get; set; }
    public string?PhoneNumber { get; set; }
    public string? LegalCode { get; set; }
    public string?ConstPhone { get; set; }
    public string?CompanyName { get; set; }
    public string? CompanyAddress { get; set; }
    public SoftwareType SoftwareType { get; set; }
    public bool IsSmsPanelActive { get; set; }
    public bool IsMobileVersionActive { get; set; }
    public int ClientCount { get; set; }
    public int AppSerialCount { get; set; }
    public bool IsActive { get; set; }
    public string? LicenseCode { get; set; }
    public string? LunarCreationTime { get; set; }
    public DateTime ChristanCreationTime { get; set; }
    
    public string? Expiration { get; set; }
    
    
}