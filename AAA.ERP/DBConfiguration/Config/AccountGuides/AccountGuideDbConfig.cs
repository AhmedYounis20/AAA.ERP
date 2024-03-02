using AAA.ERP.DBConfiguration.Config.BaseConfig;
using AAA.ERP.Models.Data.AccountGuide;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AAA.ERP.DBConfiguration.Config.Currencies
{
    public class AccountGuideDbConfig : BaseSettingEntityDbConfig<AccountGuide>
    {

        protected override EntityTypeBuilder<AccountGuide> ApplyConfiguration(EntityTypeBuilder<AccountGuide> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("AccountGuides");
            return builder;
        }
    }
}
