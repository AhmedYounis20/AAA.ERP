using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Inventory.Domain.Models.Entities.SellingPrices;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence;

public class SellingPriceDbConfig : BaseSettingEntityDbConfig<SellingPrice>
{
    protected override EntityTypeBuilder<SellingPrice> ApplyConfiguration(EntityTypeBuilder<SellingPrice> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("SellingPrices", DbSchemaNames.Inventory);
        return builder;
    }
}
