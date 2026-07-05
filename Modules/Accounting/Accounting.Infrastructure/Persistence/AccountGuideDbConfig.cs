using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Accounting.Domain.Models.Entities.AccountGuides;

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
