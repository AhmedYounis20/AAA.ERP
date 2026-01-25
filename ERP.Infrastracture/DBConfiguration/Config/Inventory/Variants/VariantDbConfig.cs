using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Account.DBConfiguration.Config.BaseConfig;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Variants
{
    public class VariantDbConfig : BaseEntityDbConfig<Variant>
    {
        protected override EntityTypeBuilder<Variant> ApplyConfiguration(EntityTypeBuilder<Variant> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("Variants");

            _ = builder.Property(e => e.ProductId).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.DefaultDiscountType).HasConversion<string>().HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.DefaultDiscount).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.ConditionalDiscount).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.MaxDiscount).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.IsDiscountBasedOnSellingPrice).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.ApplyProductChanges).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.IsDefault).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.IsUsed).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.IsArchived).HasColumnOrder(columnNumber++);

            _ = builder.HasOne(e => e.Product)
            .WithMany(e => e.Variants)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.VariantSuppliers)
            .WithOne(e => e.Variant)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.VariantCodes)
            .WithOne(e => e.Variant)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.VariantManufacturerCompanies)
            .WithOne(e => e.Variant)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.VariantPackingUnitPrices)
            .WithOne(e => e.Variant)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.VariantSellingPriceDiscounts)
            .WithOne(e => e.Variant)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.VariantAttachments)
            .WithOne(e => e.Variant)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.VariantAttributeValues)
            .WithOne(e => e.Variant)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}
