namespace CoreBussiness.BussinessEntity.Licenses;

public interface ILicenseService
{
    Task AddNewLicenseAsync(License license);
    Task<License?> GetLicenseAsync(string? licenseCode);
    Task<License?> GetLicenseAsync(int Id);
    Task<List<LicenseExclModel>> ListLicenseCodeAsync();
    Task<List<LicenseDataViewModel>> ListLicenseDataAsync(int skip, int take, string? search);
    Task<int> GetLicenseCountAsync(string?search);
    
    
}