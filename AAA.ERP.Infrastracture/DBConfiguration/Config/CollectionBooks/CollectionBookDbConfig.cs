using Domain.Account.DBConfiguration.Config.BaseConfig;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.CollectionBooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Account.DBConfiguration.Config.CollectionBooks
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