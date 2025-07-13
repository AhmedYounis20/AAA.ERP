using ERP.Domain.Models.Entities.Inventory.Sizes;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.StockBalances;

public class StockBalanceDbConfig : BaseEntityDbConfig<StockBalance>
{
    protected override EntityTypeBuilder<StockBalance> ApplyConfiguration(EntityTypeBuilder<StockBalance> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("StockBalances");

        _ = builder.Property(e => e.ItemId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.PackingUnitId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.BranchId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.CurrentBalance).IsRequired().HasColumnType("decimal(18,2)").HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.MinimumBalance).IsRequired().HasColumnType("decimal(18,2)").HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.MaximumBalance).IsRequired().HasColumnType("decimal(18,2)").HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.UnitCost).IsRequired().HasColumnType("decimal(18,2)").HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.TotalCost).IsRequired().HasColumnType("decimal(18,2)").HasColumnOrder(columnNumber++);

        // Relationships
        _ = builder.HasOne(e => e.Item).WithMany().HasForeignKey(e => e.ItemId);
        _ = builder.HasOne(e => e.PackingUnit).WithMany().HasForeignKey(e => e.PackingUnitId);
        _ = builder.HasOne(e => e.Branch).WithMany().HasForeignKey(e => e.BranchId);

        // Unique constraint for Item, PackingUnit, and Branch combination
        _ = builder.HasIndex(e => new { e.ItemId, e.PackingUnitId, e.BranchId }).IsUnique();

        return builder;
    }
} 