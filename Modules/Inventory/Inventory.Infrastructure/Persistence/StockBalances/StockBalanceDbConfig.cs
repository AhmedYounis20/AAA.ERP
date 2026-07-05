using Inventory.Domain.Models.Entities.Sizes;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.StockBalances;

public class StockBalanceDbConfig : BaseEntityDbConfig<StockBalance>
{
    protected override EntityTypeBuilder<StockBalance> ApplyConfiguration(EntityTypeBuilder<StockBalance> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("StockBalances", DbSchemaNames.Inventory);

        _ = builder.Property(e => e.VariantId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.PackingUnitId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.BranchId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.CurrentBalance).IsRequired().HasColumnType("decimal(18,2)").HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.MinimumBalance).IsRequired().HasColumnType("decimal(18,2)").HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.MaximumBalance).IsRequired().HasColumnType("decimal(18,2)").HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.UnitCost).IsRequired().HasColumnType("decimal(18,2)").HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.TotalCost).IsRequired().HasColumnType("decimal(18,2)").HasColumnOrder(columnNumber++);

        // Relationships
        _ = builder.HasOne(e => e.Variant).WithMany().HasForeignKey(e => e.VariantId);
        _ = builder.HasOne(e => e.PackingUnit).WithMany().HasForeignKey(e => e.PackingUnitId);
        _ = builder.HasOne(e => e.Branch).WithMany().HasForeignKey(e => e.BranchId);

        // Unique constraint for Variant, PackingUnit, and Branch combination
        _ = builder.HasIndex(e => new { e.VariantId, e.PackingUnitId, e.BranchId }).IsUnique();

        return builder;
    }
} 