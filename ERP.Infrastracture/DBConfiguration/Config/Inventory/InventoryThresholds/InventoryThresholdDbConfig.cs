using ERP.Domain.Models.Entities.Inventory.InventoryThresholds;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.InventoryThresholds;

public class InventoryThresholdDbConfig : BaseEntityDbConfig<InventoryThreshold>
{
    protected override EntityTypeBuilder<InventoryThreshold> ApplyConfiguration(EntityTypeBuilder<InventoryThreshold> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("InventoryThresholds");

        _ = builder.Property(e => e.Level)
            .IsRequired();
        _ = builder.Property(e => e.MinQuantity)
            .HasColumnType("decimal(18,4)")
            .IsRequired();
        _ = builder.Property(e => e.MaxQuantity)
            .HasColumnType("decimal(18,4)")
            .IsRequired();
        _ = builder.Property(e => e.Scope)
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();
        _ = builder.Property(e => e.OwnerType)
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();
        _ = builder.Property(e => e.EnableNotification)
            .IsRequired();

        // NO ACTION to avoid multiple cascade paths (Product -> Variants -> InventoryThresholds and Product -> InventoryThresholds)
        _ = builder.HasOne(e => e.Product)
            .WithMany(e => e.InventoryThresholds)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        _ = builder.HasOne(e => e.Variant)
            .WithMany(e => e.InventoryThresholds)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Restrict);

        _ = builder.HasIndex(e => new { e.VariantId, e.Scope, e.BranchId });
        _ = builder.HasIndex(e => new { e.ProductId, e.Scope, e.BranchId });

        return builder;
    }
}