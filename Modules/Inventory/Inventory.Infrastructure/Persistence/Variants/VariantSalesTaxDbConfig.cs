using Inventory.Domain.Models.Entities.Variants;
using ERP.BuildingBlocks.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.Variants;

public class VariantSalesTaxDbConfig : BaseEntityDbConfig<VariantSalesTax>
{
    protected override EntityTypeBuilder<VariantSalesTax> ApplyConfiguration(EntityTypeBuilder<VariantSalesTax> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("VariantSalesTaxes", DbSchemaNames.Inventory);

        _ = builder.HasIndex(e => new { e.VariantId, e.TaxId }).IsUnique();

        _ = builder.HasOne(e => e.Variant)
            .WithMany(e => e.VariantSalesTaxes)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

        return builder;
    }
}
