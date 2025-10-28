using Account.Domain.Models.Entities.ChartOfAccounts;

namespace Account.Domain.Models.Entities.CostCenters;

public class CostCenterChartOfAccount : BaseEntity
{
    public Guid CostCenterId { get; set; }

    public Guid ChartOfAccountId { get; set; }
    public ChartOfAccount ChartOfAccount { get; set; }
}
