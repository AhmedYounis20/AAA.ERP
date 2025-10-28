using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Infrastructure.Data.DbConfig.SellingPrices
{
    public class SellingPriceDbConfig : BaseSettingEntityDbConfig<SellingPrice>
    {
        protected override EntityTypeBuilder<SellingPrice> ApplyConfiguration(EntityTypeBuilder<SellingPrice> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("SellingPrices");
            return builder;
        }
    }
}