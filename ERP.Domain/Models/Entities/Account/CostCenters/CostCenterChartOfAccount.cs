using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Account.CostCenters;

public class CostCenterChartOfAccount : BaseEntity
{
    public Guid CostCenterId { get; set; }

    public Guid ChartOfAccountId { get; set; }
    public ChartOfAccount ChartOfAccount { get; set; }
}