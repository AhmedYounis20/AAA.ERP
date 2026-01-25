using ERP.Domain.Models.Entities.Inventory.Products;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Products;

public class ProductSellingPriceDiscountDbConfig : IEntityTypeConfiguration<ProductSellingPriceDiscount>
{
    public void Configure(EntityTypeBuilder<ProductSellingPriceDiscount> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Discount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
        builder.HasOne(e => e.Product)
            .WithMany(p => p.ProductSellingPriceDiscounts)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


