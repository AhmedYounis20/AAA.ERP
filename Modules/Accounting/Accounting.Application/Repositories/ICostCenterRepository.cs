using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Domain.Models.Entities.CostCenters;

namespace Accounting.Application.Repositories;

public interface ICostCenterRepository : IBaseTreeSettingRepository<CostCenter>
{
    void RemoveChartOfAccounts(List<CostCenterChartOfAccount> chartOfAccounts);
}
