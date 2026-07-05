using Domain.Account.DBConfiguration.Config.BaseConfig;
using Accounting.Domain.Models.Entities.CollectionBooks;

namespace Accounting.Infrastructure.Persistence;

public class CollectionBookDbConfig : BaseSettingEntityDbConfig<CollectionBook>
{
    protected override EntityTypeBuilder<CollectionBook> ApplyConfiguration(EntityTypeBuilder<CollectionBook> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("CollectionBooks");
        return builder;
    }
}
