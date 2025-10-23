using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Attributes;

public class AttributeValueDbConfig : IEntityTypeConfiguration<AttributeValue>
{
    public void Configure(EntityTypeBuilder<AttributeValue> builder)
    {
        builder.ToTable("AttributeValues");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.NameSecondLanguage).IsRequired().HasMaxLength(100);
        builder.Property(e => e.SortOrder);
        builder.Property(e => e.IsActive).IsRequired();

        // Configure relationships
        builder.HasOne(av => av.AttributeDefinition)
            .WithMany(ad => ad.PredefinedValues)
            .HasForeignKey(av => av.AttributeDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index for performance
        builder.HasIndex(e => new { e.AttributeDefinitionId, e.Name }).IsUnique();
    }
}