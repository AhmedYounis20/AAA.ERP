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

            _ = builder.HasMany(e => e.ItemPackingUnitSellingPrices)
            .WithOne(e=>e.ItemPackingUnitPrice)
            .HasForeignKey(e => e.ItemPackingUnitPriceId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasOne(e => e.PackingUnit)
            .WithMany()
            .HasForeignKey(e => e.PackingUnitId)
            .OnDelete(DeleteBehavior.Restrict);

            return builder;
        }
    }
}