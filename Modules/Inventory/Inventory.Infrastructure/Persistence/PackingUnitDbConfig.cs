using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;

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
