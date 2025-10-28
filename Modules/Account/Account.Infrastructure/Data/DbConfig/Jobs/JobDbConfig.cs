using Account.Domain.Models.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Infrastructure.Data.DbConfig.BaseConfig;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.Jobs;

public class RoleDbConfig : BaseSettingEntityDbConfig<Role>
{

    protected override EntityTypeBuilder<Role> ApplyConfiguration(EntityTypeBuilder<Role> builder)
    {
        builder.Property(e => e.Commission).HasColumnOrder(columnNumber++);
        base.ApplyConfiguration(builder);
        builder.ToTable("Roles");
        return builder;
    }
}
