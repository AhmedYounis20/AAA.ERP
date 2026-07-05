using Inventory.Domain.Models.Entities.Items;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.Items
{
    public class ItemPackingUnitDbConfig : BaseEntityDbConfig<ItemPackingUnit>
    {
        protected override EntityTypeBuilder<ItemPackingUnit> ApplyConfiguration(EntityTypeBuilder<ItemPackingUnit> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ItemPackingUnits", DbSchemaNames.Inventory);

            _ = builder.HasIndex(e => new { e.ItemId, e.PackingUnitId }).IsUnique();

            _ = builder.HasMany(e => e.ItemPackingUnitSellingPrices)
            .WithOne(e => e.ItemPackingUnit)
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