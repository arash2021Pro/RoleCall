namespace LicenseProject.StartupModuleServices.SqlSeedService;

public static class InitialScopeService
{
    public static void RunInitialScope(this IApplicationBuilder app)
    {
        var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
        using (var scope=scopeFactory.CreateScope())
        {
            var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
            databaseInitializer.SeedData();
        }
    }
}