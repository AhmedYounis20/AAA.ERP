using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Infrastructure.Persistence;

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

        builder.HasOne(av => av.AttributeDefinition)
            .WithMany(ad => ad.PredefinedValues)
            .HasForeignKey(av => av.AttributeDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.AttributeDefinitionId, e.Name }).IsUnique();
    }
}
