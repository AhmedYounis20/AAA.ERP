using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Account.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.Jobs
{
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
}
