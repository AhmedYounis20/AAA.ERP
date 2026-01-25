using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Products
{
    public class ProductCodeDbConfig : BaseEntityDbConfig<ProductCode>
    {
        protected override EntityTypeBuilder<ProductCode> ApplyConfiguration(EntityTypeBuilder<ProductCode> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ProductCodes");

            _ = builder.Property(e => e.CodeType).HasConversion<string>();

            _ = builder.HasOne(e => e.Product)
            .WithMany(e => e.ProductCodes)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}


