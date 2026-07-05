using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Account.CostCenters;

namespace Accounting.Application.Repositories;

public interface ICostCenterRepository : IBaseTreeSettingRepository<CostCenter>
{
    void RemoveChartOfAccounts(List<CostCenterChartOfAccount> chartOfAccounts);
}
