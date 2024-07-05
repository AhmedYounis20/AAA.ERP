using Domain.Account.DBConfiguration.Config.BaseConfig;
using Domain.Account.Models.Entities.Currencies;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Account.DBConfiguration.Config.Currencies
{
    public class CostCenterDbConfig : BaseTreeSettingEntityDbConfig<CostCenter>
    {
        protected override EntityTypeBuilder<CostCenter> ApplyConfiguration(EntityTypeBuilder<CostCenter> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("CostCenters");
            builder.HasMany(e => e.ChartOfAccounts).WithOne().HasForeignKey(e => e.CostCenterId).OnDelete(DeleteBehavior.Cascade);
            return builder;
        }
    }
}