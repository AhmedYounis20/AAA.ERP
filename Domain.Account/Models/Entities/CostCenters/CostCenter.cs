using Shared.BaseEntities;

namespace Domain.Account.Models.Entities.CostCenters;

public class CostCenter : BaseTreeSettingEntity<CostCenter>
{
    public int Percent { get; set; }

    public CostCenterType CostCenterType { get; set; }
    public List<CostCenterChartOfAccount> ChartOfAccounts { get; set; }
}