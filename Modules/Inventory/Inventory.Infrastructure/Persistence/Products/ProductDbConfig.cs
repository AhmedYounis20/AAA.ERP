using Accounting.Domain.Models.Entities.SubLeadgers;
using Inventory.Domain.Models.Entities.Products;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.Products;

public class ProductDbConfig : BaseTreeSettingEntityDbConfig<Product>
{
    protected override EntityTypeBuilder<Product> ApplyConfiguration(EntityTypeBuilder<Product> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("Products", DbSchemaNames.Inventory);

        _ = builder.Property(e => e.ProductType).HasConversion<string>();
        _ = builder.Property(e => e.DefaultDiscountType).HasConversion<string>();
        _ = builder.Property(e => e.InventoryThresholdScope)
            .HasConversion<string>()
            .HasMaxLength(16);
        
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
