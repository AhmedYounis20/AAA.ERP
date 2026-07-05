using Inventory.Domain.Models.Entities.Variants;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.Variants;

public class VariantPurchaseTaxDbConfig : BaseEntityDbConfig<VariantPurchaseTax>
{
    protected override EntityTypeBuilder<VariantPurchaseTax> ApplyConfiguration(EntityTypeBuilder<VariantPurchaseTax> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("VariantPurchaseTaxes", DbSchemaNames.Inventory);

        _ = builder.HasIndex(e => new { e.VariantId, e.TaxId }).IsUnique();

        _ = builder.HasOne(e => e.Variant)
            .WithMany(e => e.VariantPurchaseTaxes)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

        return builder;
    }
}
