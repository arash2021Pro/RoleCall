using CoreBussiness.BussinessEntity.Serials;

namespace LicenseProject.ApiModels;

public static class Convertor
{
    public static string GetLicenseType( SoftwareType softwareType)
    {
        switch (softwareType)
        {
            case SoftwareType.Basic:
                return "Basic";
            case SoftwareType.Advanced:
                return "Advanced";
            case SoftwareType.Professional:
                return "Pro";
        }
        return "";
    }
}