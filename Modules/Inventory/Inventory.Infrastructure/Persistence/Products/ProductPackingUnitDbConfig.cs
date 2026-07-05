using Inventory.Domain.Models.Entities.Products;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Products;

public class ProductPackingUnitDbConfig : IEntityTypeConfiguration<ProductPackingUnit>
{
    public void Configure(EntityTypeBuilder<ProductPackingUnit> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.LastCostPrice)
            .HasColumnType("decimal(18,2)");
        
        builder.Property(e => e.AverageCostPrice)
            .HasColumnType("decimal(18,2)");
        
        builder.HasOne(e => e.Product)
            .WithMany(p => p.ProductPackingUnits)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


