using ERP.Domain.Models.Entities.Inventory.Products;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Products;

public class ProductPackingUnitSellingPriceDbConfig : IEntityTypeConfiguration<ProductPackingUnitSellingPrice>
{
    public void Configure(EntityTypeBuilder<ProductPackingUnitSellingPrice> builder)
    {
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


