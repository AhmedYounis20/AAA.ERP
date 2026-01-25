using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Products
{
    public class ProductManufacturerCompanyDbConfig : BaseEntityDbConfig<ProductManufacturerCompany>
    {
        protected override EntityTypeBuilder<ProductManufacturerCompany> ApplyConfiguration(EntityTypeBuilder<ProductManufacturerCompany> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ProductManufacturerCompanies");

            _ = builder.HasOne(e => e.Product)
            .WithMany(e => e.ProductManufacturerCompanies)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasOne(e => e.ManufacturerCompany)
            .WithMany()
            .HasForeignKey(e => e.ManufacturerCompanyId)
            .OnDelete(DeleteBehavior.Restrict);

            return builder;
        }
    }
}


