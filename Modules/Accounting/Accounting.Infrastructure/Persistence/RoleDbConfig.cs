using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Account.Roles;

namespace Accounting.Infrastructure.Persistence;

public class RoleDbConfig : BaseSettingEntityDbConfig<Role>
{
    protected override EntityTypeBuilder<Role> ApplyConfiguration(EntityTypeBuilder<Role> builder)
    {
        _ = builder.Property(e => e.Commission).HasColumnOrder(columnNumber++);
        base.ApplyConfiguration(builder);
        builder.ToTable("Roles");
        return builder;
    }
}
