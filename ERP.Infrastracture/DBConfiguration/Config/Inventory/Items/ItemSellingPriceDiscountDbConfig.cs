using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Items
{
    public class ItemSellingPriceDiscountDbConfig : BaseEntityDbConfig<ItemSellingPriceDiscount>
    {
        protected override EntityTypeBuilder<ItemSellingPriceDiscount> ApplyConfiguration(EntityTypeBuilder<ItemSellingPriceDiscount> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ItemSellingPriceDiscounts");

            _ = builder.Property(x => x.DiscountType).HasConversion<string>();
            _ = builder.HasIndex(e => new { e.ItemId, e.SellingPriceId }).IsUnique();

            _ = builder.HasOne(e => e.SellingPrice)
            .WithMany()
            .HasForeignKey(e => e.SellingPriceId)
            .OnDelete(DeleteBehavior.Restrict);

            return builder;
        }
    }
}