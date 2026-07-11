using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Domain.Entities;

namespace Accounting.Domain.Models.Entities.CostCenters;

public class CostCenterChartOfAccount : BaseEntity
{
    public Guid CostCenterId { get; set; }

    public Guid ChartOfAccountId { get; set; }
    public ChartOfAccount ChartOfAccount { get; set; }
}