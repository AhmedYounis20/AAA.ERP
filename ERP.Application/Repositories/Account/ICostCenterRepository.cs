using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Account.CostCenters;

namespace ERP.Application.Repositories.Account;

public interface ICostCenterRepository : IBaseTreeSettingRepository<CostCenter>
{
    public void RemoveChartOfAccounts(List<CostCenterChartOfAccount> chartOfAccounts);
}
