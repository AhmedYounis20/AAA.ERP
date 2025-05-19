using Domain.Account.Models.Entities.CostCenters;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Account.DBConfiguration.Config.Currencies
{
    public class CostCenterChartOfAccountDbConfig : BaseEntityDbConfig<CostCenterChartOfAccount>
    {

        protected override EntityTypeBuilder<CostCenterChartOfAccount> ApplyConfiguration(EntityTypeBuilder<CostCenterChartOfAccount> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("CostCenterChartOfAccounts");

            builder.HasOne(e => e.ChartOfAccount).WithMany().HasForeignKey(e=>e.ChartOfAccountId);
            builder.HasIndex(e => new {e.ChartOfAccountId, e.CostCenterId}).IsUnique();
            return builder;
        }
    }
}
