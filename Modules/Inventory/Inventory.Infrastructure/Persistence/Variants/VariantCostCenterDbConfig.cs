using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Variants;

public class VariantCostCenterDbConfig : BaseEntityDbConfig<VariantCostCenter>
{
    protected override EntityTypeBuilder<VariantCostCenter> ApplyConfiguration(EntityTypeBuilder<VariantCostCenter> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("VariantCostCenters");

        _ = builder.Property(e => e.Percent)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        _ = builder.HasIndex(e => new { e.VariantId, e.CostCenterId }).IsUnique();

        _ = builder.HasOne(e => e.Variant)
            .WithMany(e => e.VariantCostCenters)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = builder.HasOne(e => e.CostCenter)
            .WithMany()
            .HasForeignKey(e => e.CostCenterId)
            .OnDelete(DeleteBehavior.Restrict);

        return builder;
    }
}
