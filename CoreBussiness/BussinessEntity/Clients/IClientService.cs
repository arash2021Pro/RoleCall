namespace CoreBussiness.BussinessEntity.Clients;

public interface IClientService
{
    Task AddNewClientAsync(Client client);
    Task<Client?> GetClientAsync(int id);
    Task<List<ClientDataViewModel>> ListClientDataAsync(int licenseId,int skip, int take, string? search);
    Task<int> GetClientCountAsync(string?search);
    Task<List<Client>> ListClientAsync(int licenseId);
    Task<Client?> CatchClientAsync(int licenseId);
    Task<Client?> GetClientAsync(string? licenseSerial,int licenseId);
}