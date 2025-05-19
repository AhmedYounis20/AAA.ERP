using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Account.CostCenters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.CostCenters
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