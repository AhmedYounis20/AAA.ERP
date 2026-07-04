using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Account.AccountGuides;

namespace Accounting.Infrastructure.Persistence;

public class AccountGuideDbConfig : BaseSettingEntityDbConfig<AccountGuide>
{
    protected override EntityTypeBuilder<AccountGuide> ApplyConfiguration(EntityTypeBuilder<AccountGuide> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("AccountGuides");
        return builder;
    }
}
