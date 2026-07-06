using ERP.BuildingBlocks.Persistence.Configurations;
using Accounting.Domain.Models.Entities.AccountGuides;
using ERP.BuildingBlocks.Persistence;

namespace Accounting.Infrastructure.Persistence;

public class AccountGuideDbConfig : BaseSettingEntityDbConfig<AccountGuide>
{
    protected override EntityTypeBuilder<AccountGuide> ApplyConfiguration(EntityTypeBuilder<AccountGuide> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("AccountGuides", DbSchemaNames.Accounting);
        return builder;
    }
}
