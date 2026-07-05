using Inventory.Domain.Models.Entities.Variants;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.Variants;

public class VariantCostCenterDbConfig : BaseEntityDbConfig<VariantCostCenter>
{
    protected override EntityTypeBuilder<VariantCostCenter> ApplyConfiguration(EntityTypeBuilder<VariantCostCenter> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("VariantCostCenters", DbSchemaNames.Inventory);

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
