using ERP.Domain.Models.Entities.Inventory;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory;

public class InventoryTransferItemDbConfig : IEntityTypeConfiguration<InventoryTransferItem>
{
    public void Configure(EntityTypeBuilder<InventoryTransferItem> builder)
    {
        builder.ToTable("InventoryTransferItems");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Quantity).IsRequired();
        builder.HasOne(e => e.Item)
            .WithMany()
            .HasForeignKey(e => e.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.PackingUnit)
            .WithMany()
            .HasForeignKey(e => e.PackingUnitId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.InventoryTransfer)
            .WithMany(e => e.Items)
            .HasForeignKey(e => e.InventoryTransferId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 