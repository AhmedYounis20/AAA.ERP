using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Models.Entities.AccountGuide;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace AAA.ERP.DBConfiguration.Config.BaseConfig;

public class BaseSettingEntityDbConfig<TEntity> : BaseEntityDbConfig<TEntity> where TEntity : BaseSettingEntity
{
    protected override EntityTypeBuilder<TEntity> ApplyConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        _ = builder.Property(e => e.Name).IsRequired().HasMaxLength(50).HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.NameSecondLanguage).HasMaxLength(50).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.HasIndex(e => e.Name).IsUnique();
        _ = builder.HasIndex(e => e.NameSecondLanguage).IsUnique();

        return builder;
    }
}
