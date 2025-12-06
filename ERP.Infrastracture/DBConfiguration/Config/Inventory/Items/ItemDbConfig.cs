using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Items
{
    public class ItemDbConfig : BaseTreeSettingEntityDbConfig<Item>
    {
        protected override EntityTypeBuilder<Item> ApplyConfiguration(EntityTypeBuilder<Item> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("Items");

            _ = builder.Property(e => e.ItemType).HasConversion<string>();
            _ = builder.Property(e => e.DefaultDiscountType).HasConversion<string>();
            
            // Indexes for frequently queried columns
            _ = builder.HasIndex(e => e.NodeType);
            _ = builder.HasIndex(e => e.ParentId);
            _ = builder.HasIndex(e => new { e.ParentId, e.NodeType });

            _ = builder.HasMany(e => e.ItemSuppliers)
            .WithOne(e => e.Item)
            .HasForeignKey(e => e.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.ItemCodes)
            .WithOne(e => e.Item)
            .HasForeignKey(e => e.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.ItemManufacturerCompanies)
            .WithOne(e => e.Item)
            .HasForeignKey(e => e.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.ItemPackingUnitPrices)
            .WithOne(e => e.Item)
            .HasForeignKey(e => e.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.ItemSellingPriceDiscounts)
            .WithOne(e => e.Item)
            .HasForeignKey(e => e.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}