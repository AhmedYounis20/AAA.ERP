using ERP.Domain.Models.Entities.Inventory.Products;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Products;

public class ProductAttributeDefinitionDbConfig : IEntityTypeConfiguration<ProductAttributeDefinition>
{
    public void Configure(EntityTypeBuilder<ProductAttributeDefinition> builder)
    {
        builder.ToTable("ProductAttributeDefinitions");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.IsVariant).IsRequired();

        // Configure relationships
        builder.HasOne(pad => pad.Product)
            .WithMany(p => p.ProductAttributeDefinitions)
            .HasForeignKey(pad => pad.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pad => pad.AttributeDefinition)
            .WithMany(ad => ad.ProductAttributes)
            .HasForeignKey(pad => pad.AttributeDefinitionId)
            .OnDelete(DeleteBehavior.NoAction);

        // Unique constraint - a product can only have one relationship with each attribute definition
        builder.HasIndex(e => new { e.ProductId, e.AttributeDefinitionId }).IsUnique();
    }
}



