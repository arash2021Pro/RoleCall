using System.Xml.Serialization;

namespace LicenseProject.LicenseService;

public class XmlLicense
{
  
    
    [XmlAttribute]
    public DateTime Expiration { set; get; }

    [XmlAttribute]
    public LicenseType Type { set; get; }

    [XmlAttribute]
    public string? EnCompanyName { set; get; }

    [XmlAttribute]
    public string? FaCompanyName { set; get; }


    [XmlAttribute]
    public bool EnableSms { set; get; }
    [XmlAttribute]
    public bool EnableMobile { set; get; }

    [XmlAttribute]
    public string? LicenseNo { set; get; }

    [XmlAttribute]
    public string? SystemCode { set; get; }
    
    [XmlAttribute]
    public bool IsActive { get; set; }

}

public enum LicenseType
{
    Basic,
    Advanced,
    Pro,
    
}