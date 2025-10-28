namespace Inventory.Infrastructure.Data.DbConfig.Items;

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
