using Account.Domain.Models.Entities.AccountGuides;

namespace Account.Infrastructure.Data.DbConfig.AccountGuides;

public class AccountGuideDbConfig : BaseSettingEntityDbConfig<AccountGuide>
{

    protected override EntityTypeBuilder<AccountGuide> ApplyConfiguration(EntityTypeBuilder<AccountGuide> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("AccountGuides");
        return builder;
    }
}
