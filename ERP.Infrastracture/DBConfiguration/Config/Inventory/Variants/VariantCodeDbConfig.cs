using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Variants
{
    public class VariantCodeDbConfig : BaseEntityDbConfig<VariantCode>
    {
        protected override EntityTypeBuilder<VariantCode> ApplyConfiguration(EntityTypeBuilder<VariantCode> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("VariantCodes");

            _ = builder.Property(e => e.CodeType).HasConversion<string>();

            _ = builder.HasOne(e => e.Variant)
            .WithMany(e => e.VariantCodes)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}


