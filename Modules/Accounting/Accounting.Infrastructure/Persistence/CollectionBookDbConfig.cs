using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.BuildingBlocks.Persistence;

namespace Accounting.Infrastructure.Persistence;

public class CollectionBookDbConfig : BaseSettingEntityDbConfig<CollectionBook>
{
    protected override EntityTypeBuilder<CollectionBook> ApplyConfiguration(EntityTypeBuilder<CollectionBook> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("CollectionBooks", DbSchemaNames.Accounting);
        return builder;
    }
}
