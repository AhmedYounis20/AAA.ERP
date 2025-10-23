using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Attributes;

public class AttributeDefinitionDbConfig : IEntityTypeConfiguration<AttributeDefinition>
{
    public void Configure(EntityTypeBuilder<AttributeDefinition> builder)
    {
        builder.ToTable("AttributeDefinitions");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.NameSecondLanguage).IsRequired().HasMaxLength(100);
        builder.Property(e => e.IsActive).IsRequired();
        builder.Property(e => e.SortOrder).IsRequired();

        // Unique constraints
        builder.HasIndex(e => e.Name).IsUnique();
        builder.HasIndex(e => e.NameSecondLanguage).IsUnique();

        // Configure relationships
        builder.HasMany(ad => ad.PredefinedValues)
            .WithOne(av => av.AttributeDefinition)
            .HasForeignKey(av => av.AttributeDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}





