using Inventory.Domain.Models.Entities.Products;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Products;

public class ProductPurchaseTaxDbConfig : BaseEntityDbConfig<ProductPurchaseTax>
{
    protected override EntityTypeBuilder<ProductPurchaseTax> ApplyConfiguration(EntityTypeBuilder<ProductPurchaseTax> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("ProductPurchaseTaxes");

        _ = builder.HasIndex(e => new { e.ProductId, e.TaxId }).IsUnique();

        _ = builder.HasOne(e => e.Product)
            .WithMany(e => e.ProductPurchaseTaxes)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        return builder;
    }
}
