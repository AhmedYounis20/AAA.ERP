using AAA.ERP.Models.BaseEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AAA.ERP.DBConfiguration.Config.BaseConfig;

public class BaseTreeSettingEntityDbConfig<TEntity> : BaseTreeEntityDbConfig<TEntity> where TEntity : BaseTreeSettingEntity
{
    protected override EntityTypeBuilder<TEntity> ApplyConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        base.ApplyConfiguration(builder);

        _ = builder.Property(e => e.Name).IsRequired().HasMaxLength(50).HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.NameSecondLanguage).HasMaxLength(50).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.HasIndex(e => e.Name).IsUnique();
        _ = builder.HasIndex(e => e.NameSecondLanguage).IsUnique();

        return builder;
    }
}