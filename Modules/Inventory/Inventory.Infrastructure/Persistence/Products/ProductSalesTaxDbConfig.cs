using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Products;

public class ProductSalesTaxDbConfig : BaseEntityDbConfig<ProductSalesTax>
{
    protected override EntityTypeBuilder<ProductSalesTax> ApplyConfiguration(EntityTypeBuilder<ProductSalesTax> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("ProductSalesTaxes");

        _ = builder.HasIndex(e => new { e.ProductId, e.TaxId }).IsUnique();

        _ = builder.HasOne(e => e.Product)
            .WithMany(e => e.ProductSalesTaxes)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        return builder;
    }
}
