using CoreBussiness.BussinessEntity.Licenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreStorage.EntityConfigurations;

public class LicenseConfiguration:IEntityTypeConfiguration<License>
{
    public void Configure(EntityTypeBuilder<License> builder)
    {
        builder.Property(x => x.Name).IsRequired(false);
        builder.Property(x => x.LastName).IsRequired(false);
        builder.Property(x => x.ConstPhone).IsRequired(false).HasMaxLength(12);
        //legalcode is either nationalcode or idebtity of company
        builder.Property(x => x.LegalCode).IsRequired(false).HasMaxLength(10);
        builder.Property(x => x.PhoneNumber).IsRequired(false).HasMaxLength(11);
        builder.Property(x => x.CompanyName).IsRequired(false);
        builder.Property(x => x.CompanyAddress).IsRequired(false);
    }
}