using CoreStorage.StorageContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.NodaTime.Extensions;

namespace LicenseProject.StartupModuleServices.SqlStorageService;

public static class SqlserverService
{
    public static void RunSqlServerService(this IServiceCollection service, IConfiguration configuration)
    {
        var StorageUrl = configuration.GetConnectionString("DefaultConnection");
        service.AddDbContextPool<ApplicationContext>(option =>
        {
            option.UseSqlServer(StorageUrl, x =>
            {
                x.EnableRetryOnFailure(3);
                x.MinBatchSize(5).MaxBatchSize(50);
            }).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            option.AddInterceptors();
            option.LogTo(Console.WriteLine);
        });
    }
}