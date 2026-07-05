using Inventory.Domain.Models.Entities.Variants;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Variants
{
    public class VariantPackingUnitSellingPriceDbConfig : BaseEntityDbConfig<VariantPackingUnitSellingPrice>
    {
        protected override EntityTypeBuilder<VariantPackingUnitSellingPrice> ApplyConfiguration(EntityTypeBuilder<VariantPackingUnitSellingPrice> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("VariantPackingUnitSellingPrices");

            _ = builder.HasOne(e => e.VariantPackingUnit)
            .WithMany(e => e.VariantPackingUnitSellingPrices)
            .HasForeignKey(e => e.VariantPackingUnitId)
            .OnDelete(DeleteBehavior.Cascade);


            return builder;
        }
    }
}


