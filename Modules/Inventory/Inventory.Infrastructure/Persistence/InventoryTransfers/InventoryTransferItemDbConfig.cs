using Inventory.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.InventoryTransfers;

public class InventoryTransferItemDbConfig : IEntityTypeConfiguration<InventoryTransferItem>
{
    public void Configure(EntityTypeBuilder<InventoryTransferItem> builder)
    {
        builder.ToTable("InventoryTransferItems");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Quantity).IsRequired();
        builder.HasOne(e => e.Variant)
            .WithMany()
            .HasForeignKey(e => e.VariantId)
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
