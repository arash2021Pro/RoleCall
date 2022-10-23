
namespace LicenseProject.StartupModuleServices.CodeGenerators;

public class CodeGeneratorService:ICodeGeneratorService
{
    public string GenerateOtpCode(int len)
    {
        var code = "";
        var randome = new Random();
        for (int i = 0; i <= len; i++)
        {
            code += randome.Next(1, 10);
        }
        return code;
    }

    public string GenerateLicenseCode(int minLen, int maxLen) => Guid.NewGuid().ToString().Replace("-","").Substring(minLen = 1, maxLen).ToUpper();
   
}