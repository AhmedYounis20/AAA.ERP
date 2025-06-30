using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Sizes
{
    public class SizeDbConfig : BaseSettingEntityDbConfig<Size>
    {

        protected override EntityTypeBuilder<Size> ApplyConfiguration(EntityTypeBuilder<Size> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("Sizes");
            return builder;
        }
    }
}