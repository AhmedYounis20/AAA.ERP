using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Products
{
    public class ProductSupplierDbConfig : BaseEntityDbConfig<ProductSupplier>
    {
        protected override EntityTypeBuilder<ProductSupplier> ApplyConfiguration(EntityTypeBuilder<ProductSupplier> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ProductSuppliers");

            _ = builder.HasOne(e => e.Product)
            .WithMany(e => e.ProductSuppliers)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}


