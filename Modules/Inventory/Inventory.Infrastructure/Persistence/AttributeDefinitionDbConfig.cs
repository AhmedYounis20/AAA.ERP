using Inventory.Domain.Models.Entities.Attributes;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence;

public class AttributeDefinitionDbConfig : IEntityTypeConfiguration<AttributeDefinition>
{
    public void Configure(EntityTypeBuilder<AttributeDefinition> builder)
    {
        builder.ToTable("AttributeDefinitions", DbSchemaNames.Inventory);
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.NameSecondLanguage).IsRequired().HasMaxLength(100);
        builder.Property(e => e.IsActive).IsRequired();
        builder.Property(e => e.SortOrder).IsRequired();

        builder.HasIndex(e => e.Name).IsUnique();
        builder.HasIndex(e => e.NameSecondLanguage).IsUnique();

        builder.HasMany(ad => ad.PredefinedValues)
            .WithOne(av => av.AttributeDefinition)
            .HasForeignKey(av => av.AttributeDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
