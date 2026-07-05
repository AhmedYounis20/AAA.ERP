using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Variants;

public class VariantSalesTaxDbConfig : BaseEntityDbConfig<VariantSalesTax>
{
    protected override EntityTypeBuilder<VariantSalesTax> ApplyConfiguration(EntityTypeBuilder<VariantSalesTax> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("VariantSalesTaxes");

        _ = builder.HasIndex(e => new { e.VariantId, e.TaxId }).IsUnique();

        _ = builder.HasOne(e => e.Variant)
            .WithMany(e => e.VariantSalesTaxes)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

        return builder;
    }
}
