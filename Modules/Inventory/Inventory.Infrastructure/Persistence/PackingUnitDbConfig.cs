using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Infrastructure.Persistence;

public class PackingUnitDbConfig : BaseSettingEntityDbConfig<PackingUnit>
{
    protected override EntityTypeBuilder<PackingUnit> ApplyConfiguration(EntityTypeBuilder<PackingUnit> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("PackingUnits");
        return builder;
    }
}
