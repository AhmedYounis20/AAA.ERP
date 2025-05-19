using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.SellingPrices
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