using Domain.Account.Models.Entities.ChartOfAccounts;
using Shared.BaseEntities;

namespace Domain.Account.Models.Entities.CostCenters;

public class CostCenterChartOfAccount : BaseEntity
{
    public Guid CostCenterId { get; set; }

    public Guid ChartOfAccountId { get; set; }
    public ChartOfAccount ChartOfAccount { get; set; }
}