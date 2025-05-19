using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Account.CostCenters;

public class CostCenter : BaseTreeSettingEntity<CostCenter>
{
    public int Percent { get; set; }

    public CostCenterType CostCenterType { get; set; }
    public List<CostCenterChartOfAccount> ChartOfAccounts { get; set; }
}