using Domain.Account.DBConfiguration.Config.BaseConfig;
using Domain.Account.Models.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Account.DBConfiguration.Config.AccountGuides
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
