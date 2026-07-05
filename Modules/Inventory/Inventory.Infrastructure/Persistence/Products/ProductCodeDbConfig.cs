using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Products
{
    public class ProductCodeDbConfig : BaseEntityDbConfig<ProductCode>
    {
        protected override EntityTypeBuilder<ProductCode> ApplyConfiguration(EntityTypeBuilder<ProductCode> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ProductCodes");

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


