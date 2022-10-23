namespace LicenseProject.ApiModels;

public class LicenseResponse
{
    public bool IsExists { get; set; }
    public bool IsValid { get; set; }
    public string? SerialNo { get; set; }
    public string? SystemCode { get; set; }
    public string? ExpireDate { get; set; }
    public string? LicenseType { get; set; }
    public string? CompanyName { get; set; }
    public string?LicenseKey { get; set; }
    public bool IsSms { get; set; }
    public bool IsMobile { get; set; }
    public bool HasData { get; set; }
    public bool IsOtpConfirmed { get; set; }

}