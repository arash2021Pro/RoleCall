namespace CoreBussiness.BussinessEntity.OTP;

public interface IOtpService
{
    Task AddNewOtpAsync(Otp otp);
    Task<Otp?> GetOtpAsync(string? code);
}