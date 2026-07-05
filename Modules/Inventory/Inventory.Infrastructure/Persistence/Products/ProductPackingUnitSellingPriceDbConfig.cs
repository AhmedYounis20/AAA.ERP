using ERP.BuildingBlocks.Persistence;
using Inventory.Domain.Models.Entities.Products;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Products;

public class ProductPackingUnitSellingPriceDbConfig : IEntityTypeConfiguration<ProductPackingUnitSellingPrice>
{
    public void Configure(EntityTypeBuilder<ProductPackingUnitSellingPrice> builder)
    {
        builder.ToTable("ProductPackingUnitSellingPrices", DbSchemaNames.Inventory);
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
        builder.HasOne(e => e.ProductPackingUnit)
            .WithMany(p => p.ProductPackingUnitSellingPrices)
            .HasForeignKey(e => e.ProductPackingUnitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


