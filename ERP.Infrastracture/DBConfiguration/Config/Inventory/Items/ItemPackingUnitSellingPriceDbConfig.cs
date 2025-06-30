using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Items
{
    public class ItemPackingUnitSellingPriceDbConfig : BaseEntityDbConfig<ItemPackingUnitSellingPrice>
    {
        protected override EntityTypeBuilder<ItemPackingUnitSellingPrice> ApplyConfiguration(EntityTypeBuilder<ItemPackingUnitSellingPrice> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ItemPackingUnitSellingPrices");

            _ = builder.HasIndex(e => new { e.ItemPackingUnitId, e.SellingPriceId }).IsUnique();

            _ = builder.HasOne(e => e.SellingPrice)
            .WithMany()
            .HasForeignKey(e => e.SellingPriceId)
            .OnDelete(DeleteBehavior.Restrict);

            return builder;
        }
    }
}