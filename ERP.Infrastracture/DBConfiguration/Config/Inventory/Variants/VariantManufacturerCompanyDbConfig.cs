using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Variants
{
    public class VariantManufacturerCompanyDbConfig : BaseEntityDbConfig<VariantManufacturerCompany>
    {
        protected override EntityTypeBuilder<VariantManufacturerCompany> ApplyConfiguration(EntityTypeBuilder<VariantManufacturerCompany> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("VariantManufacturerCompanies");

            _ = builder.HasOne(e => e.Variant)
            .WithMany(e => e.VariantManufacturerCompanies)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasOne(e => e.ManufacturerCompany)
            .WithMany()
            .HasForeignKey(e => e.ManufacturerCompanyId)
            .OnDelete(DeleteBehavior.Restrict);

            return builder;
        }
    }
}


