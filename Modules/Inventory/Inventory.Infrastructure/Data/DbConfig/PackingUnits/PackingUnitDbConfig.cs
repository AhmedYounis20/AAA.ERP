using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Infrastructure.Data.DbConfig.PackingUnits
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