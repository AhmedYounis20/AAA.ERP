using ERP.Domain.Models.Entities.Inventory.Variants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Variants;

public class VariantAttributeValueDbConfig : IEntityTypeConfiguration<VariantAttributeValue>
{
    public void Configure(EntityTypeBuilder<VariantAttributeValue> builder)
    {
        builder.ToTable("VariantAttributeValues");
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
    }
}



