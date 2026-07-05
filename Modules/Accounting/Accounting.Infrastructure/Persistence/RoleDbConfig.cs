using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Accounting.Domain.Models.Entities.Roles;
using ERP.BuildingBlocks.Persistence;

namespace Accounting.Infrastructure.Persistence;

public class RoleDbConfig : BaseSettingEntityDbConfig<Role>
{
    protected override EntityTypeBuilder<Role> ApplyConfiguration(EntityTypeBuilder<Role> builder)
    {
        _ = builder.Property(e => e.Commission).HasColumnOrder(columnNumber++);
        base.ApplyConfiguration(builder);
        builder.ToTable("Roles", DbSchemaNames.Accounting);
        return builder;
    }
}
