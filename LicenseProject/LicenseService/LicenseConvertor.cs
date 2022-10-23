using System.Text;
using CoreBussiness.BussinessEntity.Clients;
using CoreBussiness.BussinessEntity.Licenses;

namespace LicenseProject.LicenseService;

public static class LicenseConvertor
{
    
    public static byte[]  ConvertLicense(License license, Client client)
    {
        var xmlModel = new XmlLicense();
        xmlModel.Expiration = license.Expiration!.Value;
        xmlModel.Type = (LicenseType) license.SoftwareType;
        xmlModel.EnableMobile = license.IsMobileVersionActive;
        xmlModel.EnableSms = license.IsSmsPanelActive;
        xmlModel.IsActive = license.IsActive;
        xmlModel.SystemCode = client.SystemSerial;
        xmlModel.LicenseNo = license.LicenseCode;
        xmlModel.EnCompanyName = license.CompanyName;
        var data =  LicenseGenerator.CreateLicense(xmlModel);
       return  Encoding.UTF8.GetBytes(data);
    }

    public static string ConvertLicenseKey(License license, Client client)
    {
        
        var xmlModel = new XmlLicense();
        xmlModel.Expiration = license.Expiration!.Value;
        xmlModel.Type = (LicenseType) license.SoftwareType;
        xmlModel.EnableMobile = license.IsMobileVersionActive;
        xmlModel.EnableSms = license.IsSmsPanelActive;
        xmlModel.IsActive = license.IsActive;
        xmlModel.SystemCode = client.SystemSerial;
        xmlModel.LicenseNo = license.LicenseCode;
        xmlModel.EnCompanyName = license.CompanyName;
        return LicenseGenerator.CreateLicense(xmlModel);
    }
}