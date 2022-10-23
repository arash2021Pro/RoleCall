using CoreBussiness.BussinessEntity.Clients;
using CoreBussiness.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.ClientsApplication;

public class ClientService:IClientService
{
    private DbSet<Client> _clients;
    public ClientService(IUnitOfWork work)
    {
        _clients = work.Set<Client>();
    }

    public async Task AddNewClientAsync(Client client) => await _clients.AddAsync(client);
    public async Task<Client?> GetClientAsync(int id) => await _clients.FirstOrDefaultAsync(x => x.Id == id);
    
    
    public async Task<List<ClientDataViewModel>> ListClientDataAsync(int licenseId,int skip, int take, string? search)
    {
        var data = _clients
            .Where(x => x.LicenseId == licenseId)
            .Select(x => new ClientDataViewModel()
        {
            Id = x.Id,AppSerial = x.AppSerial, CreationTime = x.CreationTime, SystemSerial = x.SystemSerial
        });
        if (!String.IsNullOrEmpty(search))
            data = data.Where(x => x.AppSerial!.Contains(search) || x.SystemSerial!.Contains(search));
        return await data.OrderByDescending(x=>x.Id).Skip(skip).Take(take).ToListAsync();
    }
    public async Task<int> GetClientCountAsync(string? search)
    {
        var clientsCount = _clients.AsQueryable();
        if (String.IsNullOrEmpty(search))
            clientsCount = clientsCount.Where(x => x.AppSerial!.Contains(search) || x.SystemSerial!.Contains(search));
        return await clientsCount.CountAsync();
    }

    public async Task<List<Client>> ListClientAsync(int licenseId)
    {
        return await _clients.Where(x => x.LicenseId == licenseId).ToListAsync();
    }

    public async Task<Client?> CatchClientAsync(int licenseId) =>
        await _clients.AsTracking().FirstOrDefaultAsync(x => x.LicenseId == licenseId);

    public async Task<Client?> GetClientAsync(string? licenseSerial, int licenseId) => await _clients.AsTracking()
        .FirstOrDefaultAsync(x => x.AppSerial == licenseSerial && x.LicenseId == licenseId);
}