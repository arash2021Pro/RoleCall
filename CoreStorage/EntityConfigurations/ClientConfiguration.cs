using CoreBussiness.BussinessEntity.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreStorage.EntityConfigurations;

public class ClientConfiguration:IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasOne(x => x.License).WithMany(x => x.Clients).HasForeignKey(x => x.LicenseId);
        builder.Property(x => x.AppSerial).IsRequired(false).HasMaxLength(8);
        // if it's needed , this one should be updated ... HasMaxLength
        builder.Property(x => x.SystemSerial).IsRequired(false);
    }
}