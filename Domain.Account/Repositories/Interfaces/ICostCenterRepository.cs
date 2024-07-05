using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Repositories.BaseRepositories.Interfaces;

namespace Domain.Account.Repositories.Interfaces;

public interface ICostCenterRepository : IBaseTreeSettingRepository<CostCenter>
{
    public void RemoveChartOfAccounts(List<CostCenterChartOfAccount> chartOfAccounts);
}
