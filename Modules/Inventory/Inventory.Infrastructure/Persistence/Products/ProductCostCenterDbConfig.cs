using Accounting.Domain.Models.Entities.CostCenters;
using Inventory.Domain.Models.Entities.Products;
using ERP.BuildingBlocks.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.Products;

public class ProductCostCenterDbConfig : BaseEntityDbConfig<ProductCostCenter>
{
    protected override EntityTypeBuilder<ProductCostCenter> ApplyConfiguration(EntityTypeBuilder<ProductCostCenter> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("ProductCostCenters", DbSchemaNames.Inventory);

        _ = builder.Property(e => e.Percent)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        _ = builder.HasIndex(e => new { e.ProductId, e.CostCenterId }).IsUnique();

        _ = builder.HasOne(e => e.Product)
            .WithMany(e => e.ProductCostCenters)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = builder.HasOne<CostCenter>()
            .WithMany()
            .HasForeignKey(e => e.CostCenterId)
            .OnDelete(DeleteBehavior.Restrict);

        return builder;
    }
}
