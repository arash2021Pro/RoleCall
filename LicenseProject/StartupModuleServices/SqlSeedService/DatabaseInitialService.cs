using System.Security.Cryptography;
using System.Text;
using CoreBussiness.BussinessEntity.Users;
using CoreBussiness.UnitOfWork;
using CoreStorage.StorageContext;
using Microsoft.EntityFrameworkCore;

namespace LicenseProject.StartupModuleServices.SqlSeedService;

public class DatabaseInitialService:IDatabaseInitializer
{
    private ApplicationContext _applicationContext;
    public DatabaseInitialService(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
        applicationContext.Database.Migrate();
    }
    public void SeedData()
    {
        
        if (!_applicationContext.Users.Any())
        {
            var user = new User() {UserName = "arash", Password = GenerateHashedPassword("1380")};
            _applicationContext.Users.Add(user);
            _applicationContext.SaveChanges();
        }
    }
    public string GenerateHashedPassword(string? password)
    {
        if (String.IsNullOrEmpty(password))
            return "";
        using var sha = new SHA512Managed();
        var bytes = Encoding.ASCII.GetBytes(password);
        var encripted = sha.ComputeHash(bytes);
        return Encoding.ASCII.GetString(encripted);
    }
    
    
}