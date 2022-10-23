namespace LicenseProject.StartupModuleServices.CodeGenerators;

public interface ICodeGeneratorService
{
    string GenerateOtpCode(int len);
    string GenerateLicenseCode(int minLen,int maxLen);
    
}