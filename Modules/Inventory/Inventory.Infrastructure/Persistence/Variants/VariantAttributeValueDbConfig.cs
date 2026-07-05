using Inventory.Domain.Models.Entities.Variants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.Variants;

public class VariantAttributeValueDbConfig : IEntityTypeConfiguration<VariantAttributeValue>
{
    public void Configure(EntityTypeBuilder<VariantAttributeValue> builder)
    {
        builder.ToTable("VariantAttributeValues", DbSchemaNames.Inventory);
        builder.HasKey(e => e.Id);

        // Configure relationships
        builder.HasOne(vav => vav.Variant)
            .WithMany(v => v.VariantAttributeValues)
            .HasForeignKey(vav => vav.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(vav => vav.AttributeDefinition)
            .WithMany(ad => ad.VariantAttributeValues)
            .HasForeignKey(vav => vav.AttributeDefinitionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(vav => vav.AttributeValue)
            .WithMany(av => av.VariantAttributeValues)
            .HasForeignKey(vav => vav.AttributeValueId)
            .OnDelete(DeleteBehavior.NoAction);

        // Unique constraint - a variant can only have one value for each attribute definition
        builder.HasIndex(e => new { e.VariantId, e.AttributeDefinitionId }).IsUnique();

        // Additional indexes for query optimization
        // Note: VariantId and AttributeDefinitionId are already in the unique composite index above
        // Note: AttributeDefinitionId index already exists from foreign key
        // Note: AttributeValueId index already exists from foreign key
    }
}



