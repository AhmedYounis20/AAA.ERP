using Inventory.Domain.Models.Entities.Variants;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Variants
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
            _ = builder.Property(e => e.InventoryThresholdScope)
                .HasConversion<string>()
                .HasMaxLength(16);

            // Indexes for frequently queried columns
            // Note: ProductId index already exists from foreign key
            _ = builder.HasIndex(e => e.IsArchived); // For filtering in GetDtos
            _ = builder.HasIndex(e => e.IsDefault); // For filtering variants
            _ = builder.HasIndex(e => new { e.ProductId, e.IsDefault }); // Composite for filtering variants by product and default status
            _ = builder.HasIndex(e => new { e.IsArchived, e.ProductId }); // Composite for filtering archived variants by product

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
