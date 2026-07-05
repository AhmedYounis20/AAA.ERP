using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Variants
{
    public class VariantSupplierDbConfig : BaseEntityDbConfig<VariantSupplier>
    {
        protected override EntityTypeBuilder<VariantSupplier> ApplyConfiguration(EntityTypeBuilder<VariantSupplier> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("VariantSuppliers");

            _ = builder.HasOne(e => e.Variant)
            .WithMany(e => e.VariantSuppliers)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}


