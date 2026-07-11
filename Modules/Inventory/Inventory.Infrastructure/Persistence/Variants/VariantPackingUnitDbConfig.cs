using Inventory.Domain.Models.Entities.Variants;
using ERP.BuildingBlocks.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.Variants
{
    public class VariantPackingUnitDbConfig : BaseEntityDbConfig<VariantPackingUnit>
    {
        protected override EntityTypeBuilder<VariantPackingUnit> ApplyConfiguration(EntityTypeBuilder<VariantPackingUnit> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("VariantPackingUnits", DbSchemaNames.Inventory);

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


