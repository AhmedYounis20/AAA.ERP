using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Account.AccountGuides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.AccountGuides
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
