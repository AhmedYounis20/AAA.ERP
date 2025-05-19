using Domain.Account.DBConfiguration.Config.BaseConfig;
using Domain.Account.Models.Entities.AccountGuide;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Account.DBConfiguration.Config.AccountGuides
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
