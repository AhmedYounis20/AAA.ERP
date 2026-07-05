using ERP.Domain.Models.Entities.Account.CostCenters;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;

namespace Accounting.Infrastructure.Persistence;

public class CostCenterChartOfAccountDbConfig : BaseEntityDbConfig<CostCenterChartOfAccount>
{
    protected override EntityTypeBuilder<CostCenterChartOfAccount> ApplyConfiguration(EntityTypeBuilder<CostCenterChartOfAccount> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("CostCenterChartOfAccounts");

        builder.HasOne(e => e.ChartOfAccount).WithMany().HasForeignKey(e => e.ChartOfAccountId);
        builder.HasIndex(e => new { e.ChartOfAccountId, e.CostCenterId }).IsUnique();
        return builder;
    }
}
