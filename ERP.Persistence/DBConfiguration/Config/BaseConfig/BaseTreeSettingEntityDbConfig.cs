using Domain.Account.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ERP.Infrastracture.DBConfiguration.Config.BaseConfig;

public class BaseTreeSettingEntityDbConfig<TEntity> : BaseSettingEntityDbConfig<TEntity> where TEntity : BaseTreeSettingEntity<TEntity>
{
    protected override EntityTypeBuilder<TEntity> ApplyConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        base.ApplyConfiguration(builder);

        _ = builder.HasKey(e => e.Id);
        _ = builder.Property(e => e.Id).HasValueGenerator<GuidValueGenerator>().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.ParentId).HasColumnOrder(columnNumber++);
        _ = builder.HasOne<TEntity>().WithMany().HasForeignKey(e => e.ParentId);

        return builder;
    }
}