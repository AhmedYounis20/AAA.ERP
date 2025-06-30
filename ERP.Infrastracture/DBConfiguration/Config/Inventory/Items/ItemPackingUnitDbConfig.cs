using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Items
{
    public class ItemPackingUnitDbConfig : BaseEntityDbConfig<ItemPackingUnit>
    {
        protected override EntityTypeBuilder<ItemPackingUnit> ApplyConfiguration(EntityTypeBuilder<ItemPackingUnit> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ItemPackingUnits");

            _ = builder.HasIndex(e => new { e.ItemId, e.PackingUnitId }).IsUnique();

            _ = builder.HasMany(e => e.ItemPackingUnitSellingPrices)
            .WithOne(e=>e.ItemPackingUnit)
            .HasForeignKey(e => e.ItemPackingUnitId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasOne(e => e.PackingUnit)
            .WithMany()
            .HasForeignKey(e => e.PackingUnitId)
            .OnDelete(DeleteBehavior.Restrict);

            return builder;
        }
    }
}