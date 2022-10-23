using CoreBussiness.BeseEntity;
using CoreBussiness.BussinessEntity.Licenses;

namespace CoreBussiness.BussinessEntity.OTP;

public class Otp:Core
{
    private const int ExpireLimit = 15;

    public Otp()
    {
        ExpireTime = DateTimeOffset.Now.AddMinutes(ExpireLimit);
    } 
    public int LicenseId { get; set; } 
    public License License { get; set; }
    public string Code { get; set; }
    public bool IsUsed { get; set; }
    public DateTimeOffset ExpireTime { get; set; }
    public bool IsAuthentic => !IsUsed && DateTimeOffset.Now < ExpireTime;
}