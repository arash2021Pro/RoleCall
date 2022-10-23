namespace CoreBussiness.BussinessEntity.Users;

public interface IUserService
{
    Task SignupAdminAsync(User user);
    public string GenerateHashedPassword(string? password);
    Task<bool> IsAdminExistsAsync(string userName, string password);
    Task<User?> GetUserAsync(string? userName, string? Password);
    Task<User?> GetDefaultUserAsync();

}