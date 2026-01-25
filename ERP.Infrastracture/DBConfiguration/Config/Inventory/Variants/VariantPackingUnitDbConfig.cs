using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Variants
{
    public class VariantPackingUnitDbConfig : BaseEntityDbConfig<VariantPackingUnit>
    {
        protected override EntityTypeBuilder<VariantPackingUnit> ApplyConfiguration(EntityTypeBuilder<VariantPackingUnit> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("VariantPackingUnits");

            _ = builder.HasOne(e => e.Variant)
            .WithMany(e => e.VariantPackingUnitPrices)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasOne(e => e.PackingUnit)
            .WithMany()
            .HasForeignKey(e => e.PackingUnitId)
            .OnDelete(DeleteBehavior.Restrict);

            _ = builder.HasMany(e => e.VariantPackingUnitSellingPrices)
            .WithOne(e => e.VariantPackingUnit)
            .HasForeignKey(e => e.VariantPackingUnitId)
            .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}


