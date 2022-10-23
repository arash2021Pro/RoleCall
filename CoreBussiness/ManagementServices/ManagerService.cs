using CoreBussiness.BussinessEntity.Clients;
using CoreBussiness.BussinessEntity.Licenses;
using CoreBussiness.BussinessEntity.OTP;
using CoreBussiness.BussinessEntity.Users;

namespace CoreBussiness.ManagementServices;

public class ManagerService:IManagerService
{
    public ManagerService(IUserService userService, ILicenseService licenseService, IClientService clientService, IOtpService otpService)
    {
        UserService = userService;
        LicenseService = licenseService;
        ClientService = clientService;
        OtpService = otpService;
    }

    public IUserService UserService { get; set; }
    public ILicenseService LicenseService { get; set; }
    public IClientService ClientService { get; set; }
    public IOtpService OtpService { get; set; }
}