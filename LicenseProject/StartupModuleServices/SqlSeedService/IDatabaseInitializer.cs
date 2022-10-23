namespace LicenseProject.StartupModuleServices.SqlSeedService;

public interface IDatabaseInitializer
{
    void SeedData();
    string GenerateHashedPassword(string?password);
}