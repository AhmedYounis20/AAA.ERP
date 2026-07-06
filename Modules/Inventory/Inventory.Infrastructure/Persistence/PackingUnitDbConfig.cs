using ERP.BuildingBlocks.Persistence.Configurations;
using Inventory.Domain.Models.Entities.PackingUnits;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence;

public class PackingUnitDbConfig : BaseSettingEntityDbConfig<PackingUnit>
{
    protected override EntityTypeBuilder<PackingUnit> ApplyConfiguration(EntityTypeBuilder<PackingUnit> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("PackingUnits", DbSchemaNames.Inventory);
        return builder;
    }
}
