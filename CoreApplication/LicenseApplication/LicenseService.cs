using CoreBussiness.BussinessEntity.Licenses;
using CoreBussiness.UnitOfWork;
using DNTPersianUtils.Core;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.LicenseApplication;

public class LicenseService:ILicenseService
{
    private DbSet<License> _licenses;
    public LicenseService(IUnitOfWork work)
    {
        _licenses = work.Set<License>();
    }
    public async Task AddNewLicenseAsync(License license) => await _licenses.AddAsync(license);

    public async Task<License?> GetLicenseAsync(string? licenseCode) =>
        await _licenses.AsTracking().FirstOrDefaultAsync(x => x.LicenseCode == licenseCode);

    public async Task<License?> GetLicenseAsync(int Id) =>
        await _licenses.AsTracking().FirstOrDefaultAsync(x => x.Id == Id);
    
    public async Task<List<LicenseExclModel>> ListLicenseCodeAsync()
    {
        return await _licenses.Select(x => new LicenseExclModel()
        {
            Id = x.Id,
            Code = x.LicenseCode!
        }).ToListAsync();
    }



    public async Task<List<LicenseDataViewModel>> ListLicenseDataAsync(int skip, int take, string? search)
    {
        var data = _licenses.Select(l => new LicenseDataViewModel()
        {
            Id = l.Id, Name = l.Name,
            ClientCount = l.ClientCount, CompanyAddress = l.CompanyAddress,
            CompanyName = l.CompanyName, ConstPhone = l.ConstPhone, LegalCode = l.LegalCode, IsActive = l.IsActive,
            LastName = l.LastName, LicenseCode = l.LicenseCode, PhoneNumber = l.PhoneNumber,
            SoftwareType = l.SoftwareType,
            AppSerialCount = l.AppSerialCount, IsMobileVersionActive = l.IsMobileVersionActive,
            IsSmsPanelActive = l.IsSmsPanelActive, LunarCreationTime  = l.CreationTime,ChristanCreationTime = l.DateTimeCreation,
            Expiration = l.Expiration.ToString()
        });
        if (!String.IsNullOrEmpty(search))
            data = data.Where(x => x.LegalCode!.Contains(search) || x.PhoneNumber!.Contains(search));
        return await data.OrderByDescending(x=>x.Id).Skip(skip).Take(take).ToListAsync();
    }

    public async Task<int> GetLicenseCountAsync(string? search)
    {
        var licenseCount = _licenses.AsQueryable();
        if (String.IsNullOrEmpty(search))
            licenseCount = licenseCount.Where(x => x.LicenseCode!.Contains(search) || x.PhoneNumber!.Contains(search));
        return await licenseCount.CountAsync();
    }

}