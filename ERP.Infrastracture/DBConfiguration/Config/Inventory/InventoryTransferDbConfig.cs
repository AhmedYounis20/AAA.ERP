using ERP.Domain.Models.Entities.Inventory;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory;

public class InventoryTransferDbConfig : IEntityTypeConfiguration<InventoryTransfer>
{
    public void Configure(EntityTypeBuilder<InventoryTransfer> builder)
    {
        builder.ToTable("InventoryTransfers");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.TransferType).IsRequired();
        builder.HasMany(e => e.Items)
            .WithOne(e => e.InventoryTransfer)
            .HasForeignKey(e => e.InventoryTransferId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.SourceBranch)
            .WithMany()
            .HasForeignKey(e => e.SourceBranchId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.DestinationBranch)
            .WithMany()
            .HasForeignKey(e => e.DestinationBranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 