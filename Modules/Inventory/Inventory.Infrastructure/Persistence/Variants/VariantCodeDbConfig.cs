using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Variants
{
    public class VariantCodeDbConfig : BaseEntityDbConfig<VariantCode>
    {
        protected override EntityTypeBuilder<VariantCode> ApplyConfiguration(EntityTypeBuilder<VariantCode> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("VariantCodes");

            _ = builder.Property(e => e.CodeType).HasConversion<string>();

            // Indexes for frequently queried columns
            // Note: VariantId index already exists from foreign key
            _ = builder.HasIndex(e => e.CodeType); // For filtering by code type

            _ = builder.HasOne(e => e.Variant)
            .WithMany(e => e.VariantCodes)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}


