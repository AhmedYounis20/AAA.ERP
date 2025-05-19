using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.PackingUnits
{
    public class PackingUnitDbConfig : BaseSettingEntityDbConfig<PackingUnit>
    {

        protected override EntityTypeBuilder<PackingUnit> ApplyConfiguration(EntityTypeBuilder<PackingUnit> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("PackingUnits");
            return builder;
        }
    }
}