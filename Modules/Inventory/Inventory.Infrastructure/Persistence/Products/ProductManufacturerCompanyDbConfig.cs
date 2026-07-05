using Inventory.Domain.Models.Entities.Products;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.Products
{
    public class ProductManufacturerCompanyDbConfig : BaseEntityDbConfig<ProductManufacturerCompany>
    {
        protected override EntityTypeBuilder<ProductManufacturerCompany> ApplyConfiguration(EntityTypeBuilder<ProductManufacturerCompany> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ProductManufacturerCompanies", DbSchemaNames.Inventory);

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


