using Accounting.Domain.Models.Entities.SubLeadgers;
using Inventory.Domain.Models.Entities.Variants;
using ERP.BuildingBlocks.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.Variants
{
    public class VariantSupplierDbConfig : BaseEntityDbConfig<VariantSupplier>
    {
        protected override EntityTypeBuilder<VariantSupplier> ApplyConfiguration(EntityTypeBuilder<VariantSupplier> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("VariantSuppliers", DbSchemaNames.Inventory);

            _ = builder.HasOne(e => e.Variant)
            .WithMany(e => e.VariantSuppliers)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasOne<Supplier>()
                .WithMany()
                .HasForeignKey(e => e.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            return builder;
        }
    }
}
