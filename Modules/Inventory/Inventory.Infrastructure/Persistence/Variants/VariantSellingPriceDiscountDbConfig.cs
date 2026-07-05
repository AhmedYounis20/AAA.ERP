using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Variants
{
    public class VariantSellingPriceDiscountDbConfig : BaseEntityDbConfig<VariantSellingPriceDiscount>
    {
        protected override EntityTypeBuilder<VariantSellingPriceDiscount> ApplyConfiguration(EntityTypeBuilder<VariantSellingPriceDiscount> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("VariantSellingPriceDiscounts");

            _ = builder.Property(e => e.DiscountType).HasConversion<string>();

            _ = builder.HasOne(e => e.Variant)
            .WithMany(e => e.VariantSellingPriceDiscounts)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasOne(e => e.SellingPrice)
            .WithMany()
            .HasForeignKey(e => e.SellingPriceId)
            .OnDelete(DeleteBehavior.Restrict);

            return builder;
        }
    }
}


