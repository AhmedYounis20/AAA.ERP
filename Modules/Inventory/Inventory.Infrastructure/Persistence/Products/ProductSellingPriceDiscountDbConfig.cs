using ERP.BuildingBlocks.Persistence;
using Inventory.Domain.Models.Entities.Products;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Products;

public class ProductSellingPriceDiscountDbConfig : IEntityTypeConfiguration<ProductSellingPriceDiscount>
{
    public void Configure(EntityTypeBuilder<ProductSellingPriceDiscount> builder)
    {
        builder.ToTable("ProductSellingPriceDiscounts", DbSchemaNames.Inventory);
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


