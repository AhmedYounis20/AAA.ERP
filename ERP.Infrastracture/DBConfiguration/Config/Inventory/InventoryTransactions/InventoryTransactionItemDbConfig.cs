using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.InventoryTransactions;

public class InventoryTransactionItemDbConfig : BaseEntityDbConfig<InventoryTransactionItem>
{
    protected override EntityTypeBuilder<InventoryTransactionItem> ApplyConfiguration(EntityTypeBuilder<InventoryTransactionItem> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("InventoryTransactionItems");

        _ = builder.Property(e => e.InventoryTransactionId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.ItemId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.PackingUnitId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.Quantity).IsRequired().HasColumnType("decimal(18,2)").HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.TotalCost).IsRequired().HasColumnType("decimal(18,2)").HasColumnOrder(columnNumber++);

        // Relationships
        _ = builder.HasOne(e => e.Item).WithMany().HasForeignKey(e => e.ItemId);
        _ = builder.HasOne(e => e.PackingUnit).WithMany().HasForeignKey(e => e.PackingUnitId);

        return builder;
    }
} 