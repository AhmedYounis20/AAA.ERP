using Inventory.Domain.Models.Entities.InventoryTransactions;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.InventoryTransactions;

public class InventoryTransactionDbConfig : BaseEntityDbConfig<InventoryTransaction>
{
    protected override EntityTypeBuilder<InventoryTransaction> ApplyConfiguration(EntityTypeBuilder<InventoryTransaction> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("InventoryTransactions", DbSchemaNames.Inventory);

        _ = builder.Property(e => e.TransactionType).HasConversion<string>().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.TransactionDate).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.TransactionNumber).IsRequired().HasMaxLength(100).HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.DocumentNumber).HasMaxLength(100).HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.TransactionPartyId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.BranchId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.FinancialPeriodId).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.Notes).HasMaxLength(1000).HasColumnOrder(columnNumber++);

        _ = builder.HasOne(e => e.TransactionParty).WithMany().HasForeignKey(e => e.TransactionPartyId);
        _ = builder.HasOne(e => e.Branch).WithMany().HasForeignKey(e => e.BranchId);
        _ = builder.HasOne(e => e.FinancialPeriod).WithMany().HasForeignKey(e => e.FinancialPeriodId);
        _ = builder.HasMany(e => e.Items).WithOne().HasForeignKey(e => e.InventoryTransactionId).OnDelete(DeleteBehavior.Cascade);

        return builder;
    }
}
