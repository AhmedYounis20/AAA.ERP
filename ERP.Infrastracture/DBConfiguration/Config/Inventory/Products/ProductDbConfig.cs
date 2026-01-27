using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Products
{
    public class ProductDbConfig : BaseTreeSettingEntityDbConfig<Product>
    {
        protected override EntityTypeBuilder<Product> ApplyConfiguration(EntityTypeBuilder<Product> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("Products");

            _ = builder.Property(e => e.ProductType).HasConversion<string>();
            _ = builder.Property(e => e.DefaultDiscountType).HasConversion<string>();

            // Indexes for frequently queried columns
            // Note: ParentId index already exists from foreign key
            _ = builder.HasIndex(e => e.ProductType);
            _ = builder.HasIndex(e => e.NodeType);
            _ = builder.HasIndex(e => new { e.ParentId, e.NodeType });

            _ = builder.HasMany(e => e.ProductSuppliers)
            .WithOne(e => e.Product)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.ProductCodes)
            .WithOne(e => e.Product)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.ProductManufacturerCompanies)
            .WithOne(e => e.Product)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.ProductAttachments)
            .WithOne(e => e.Product)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.Variants)
            .WithOne(e => e.Product)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.ProductAttributeDefinitions)
            .WithOne(e => e.Product)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}


