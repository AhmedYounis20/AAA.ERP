using Account.Domain.Models.Entities.CollectionBooks;

namespace Account.Infrastructure.Data.DbConfig.CollectionBooks;

public class CollectionBookDbConfig : BaseSettingEntityDbConfig<CollectionBook>
{
    protected override EntityTypeBuilder<CollectionBook> ApplyConfiguration(EntityTypeBuilder<CollectionBook> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("CollectionBooks");
        return builder;
    }
}
