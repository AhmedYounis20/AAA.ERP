using Inventory.Domain.Models.Entities.Products;
using ERP.BuildingBlocks.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.Products
{
    public class ProductCodeDbConfig : BaseEntityDbConfig<ProductCode>
    {
        protected override EntityTypeBuilder<ProductCode> ApplyConfiguration(EntityTypeBuilder<ProductCode> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ProductCodes", DbSchemaNames.Inventory);

            _ = builder.Property(e => e.CodeType).HasConversion<string>();

            // Indexes for frequently queried columns
            // Note: ProductId index already exists from foreign key
            _ = builder.HasIndex(e => e.CodeType); // For filtering by code type

            _ = builder.HasOne(e => e.Product)
            .WithMany(e => e.ProductCodes)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}


