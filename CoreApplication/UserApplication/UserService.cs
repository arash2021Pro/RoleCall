using System.Security.Cryptography;
using System.Text;
using CoreBussiness.BussinessEntity.Users;
using CoreBussiness.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.UserApplication;

public class UserService:IUserService
{
    private DbSet<User> _users;
    public UserService(IUnitOfWork work )
    {
        _users = work.Set<User>();
    }

    public async Task SignupAdminAsync(User user)
    {
        user.Password = GenerateHashedPassword(user.Password);
        await _users.AddAsync(user);
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
    public async Task<bool> IsAdminExistsAsync(string? userName, string? password)
    {
        var HashedPassword = GenerateHashedPassword(password);
        return await _users.AnyAsync(x => x.UserName!.Equals(userName) && x.Password!.Equals(HashedPassword));
    }
    
    public async Task<User?> GetUserAsync(string? phonenumber, string? Password)
    {
        
        return await _users.AsTracking().FirstOrDefaultAsync(x =>
            x.UserName!.Equals(phonenumber) && x.Password!.Equals(GenerateHashedPassword(Password)));
    }

    public async Task<User?> GetDefaultUserAsync() => await _users.AsTracking().FirstOrDefaultAsync();
}