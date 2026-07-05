using Inventory.Domain.Models.Entities.InventoryTransactions;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.InventoryTransactions;

public class InventoryTransactionItemDbConfig : BaseEntityDbConfig<InventoryTransactionItem>
{
    protected override EntityTypeBuilder<InventoryTransactionItem> ApplyConfiguration(EntityTypeBuilder<InventoryTransactionItem> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("InventoryTransactionItems", DbSchemaNames.Inventory);

        _ = builder.Property(e => e.InventoryTransactionId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.VariantId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.PackingUnitId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.Quantity).IsRequired().HasColumnType("decimal(18,2)").HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.TotalCost).IsRequired().HasColumnType("decimal(18,2)").HasColumnOrder(columnNumber++);

        _ = builder.HasOne(e => e.Variant).WithMany().HasForeignKey(e => e.VariantId);
        _ = builder.HasOne(e => e.PackingUnit).WithMany().HasForeignKey(e => e.PackingUnitId);

        return builder;
    }
}
