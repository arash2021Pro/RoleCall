using CoreBussiness.BussinessEntity.OTP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreStorage.EntityConfigurations;

public class OtpConfiguration:IEntityTypeConfiguration<Otp>
{
    public void Configure(EntityTypeBuilder<Otp> builder)
    {
        builder.HasOne(x => x.License).WithMany(x => x.Otps).HasForeignKey(x => x.LicenseId);
        builder.Property(x => x.Code).IsRequired(false).HasMaxLength(6);
    }
}