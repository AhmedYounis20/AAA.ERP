using Inventory.Domain.Models.Entities.Products;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.Products
{
    public class ProductSupplierDbConfig : BaseEntityDbConfig<ProductSupplier>
    {
        protected override EntityTypeBuilder<ProductSupplier> ApplyConfiguration(EntityTypeBuilder<ProductSupplier> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ProductSuppliers", DbSchemaNames.Inventory);

            _ = builder.HasOne(e => e.Product)
            .WithMany(e => e.ProductSuppliers)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}


