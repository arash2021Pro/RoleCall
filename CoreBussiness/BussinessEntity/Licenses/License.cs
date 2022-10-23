using CoreBussiness.BeseEntity;
using CoreBussiness.BussinessEntity.Clients;
using CoreBussiness.BussinessEntity.OTP;
using CoreBussiness.BussinessEntity.Serials;

namespace CoreBussiness.BussinessEntity.Licenses;

public class License:Core
{
    public License()
    {
        Expiration = DateTime.Now.AddMonths(6);
    }
    public string?Name { get; set; }
    public string?LastName { get; set; }
    public string?PhoneNumber { get; set; }
    public string? LegalCode { get; set; }
    public string?ConstPhone { get; set; }
    public string?CompanyName { get; set; }
    public string? CompanyAddress { get; set; }
    public SoftwareType SoftwareType { get; set; }
    
    public DateTime? Expiration { get; set; }
    public bool IsSmsPanelActive { get; set; }
    public bool IsMobileVersionActive { get; set; }
    public int ClientCount { get; set; }
    public int AppSerialCount { get; set; }
    public bool IsActive { get; set; }
    public string? LicenseCode { get; set; }
    public bool IsOtpConfirmed { get; set; }
    public ICollection<Otp>Otps { get; set; }
    public ICollection<Client>Clients { get; set; }
}