using CoreApplication.ClientsApplication;
using CoreApplication.LicenseApplication;
using CoreApplication.OtpApplication;
using CoreApplication.UserApplication;
using CoreBussiness.BussinessEntity.Clients;
using CoreBussiness.BussinessEntity.Licenses;
using CoreBussiness.BussinessEntity.OTP;
using CoreBussiness.BussinessEntity.Users;
using CoreBussiness.ManagementServices;
using CoreBussiness.UnitOfWork;
using CoreStorage.StorageContext;
using LicenseProject.StartupModuleServices.CodeGenerators;
using LicenseProject.StartupModuleServices.SqlSeedService;
using MapsterMapper;

namespace LicenseProject.StartupModuleServices.ApplicationServices;

public static class AppService
{
    public static void RunAppService(this IServiceCollection service)
    {
        service.AddScoped<IUnitOfWork, ApplicationContext>();
        service.AddScoped<IMapper, Mapper>();
        service.AddScoped<IUserService, UserService>();
        service.AddScoped<IClientService, ClientService>();
        service.AddScoped<ILicenseService, CoreApplication.LicenseApplication.LicenseService>();
        service.AddScoped<IOtpService, OtpService>();
        service.AddScoped<IManagerService, ManagerService>();
        service.AddScoped<ICodeGeneratorService, CodeGeneratorService>();
        service.AddScoped<IDatabaseInitializer, DatabaseInitialService>();
    }
}