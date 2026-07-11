using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Domain.Models.Entities.CostCenters;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for cost center persistence.
/// </summary>
public interface ICostCenterRepository : IBaseTreeSettingRepository<CostCenter>
{
    /// <summary>remove chart of accounts operation.</summary>
    void RemoveChartOfAccounts(List<CostCenterChartOfAccount> chartOfAccounts);
}
