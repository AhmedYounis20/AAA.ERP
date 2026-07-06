using ERP.BuildingBlocks.Persistence.Configurations;
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
