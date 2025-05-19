using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Account.CollectionBooks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.CollectionBooks
{
    public class CollectionBookDbConfig : BaseSettingEntityDbConfig<CollectionBook>
    {
        protected override EntityTypeBuilder<CollectionBook> ApplyConfiguration(EntityTypeBuilder<CollectionBook> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("CollectionBooks");
            return builder;
        }
    }
}