using CoreBussiness.BussinessEntity.Clients;
using CoreBussiness.BussinessEntity.Licenses;
using CoreBussiness.BussinessEntity.OTP;
using CoreBussiness.BussinessEntity.Users;

namespace CoreBussiness.ManagementServices;

public interface IManagerService
{
    public IUserService UserService { get; set; }
    public ILicenseService LicenseService { get; set; }
    public IClientService ClientService { get; set; }
    public IOtpService OtpService { get; set; }
}