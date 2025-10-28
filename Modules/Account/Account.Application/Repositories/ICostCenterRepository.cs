using Account.Domain.Models.Entities.CostCenters;
using Shared.Application.Repositories;

namespace Account.Application.Repositories;

public interface ICostCenterRepository : IBaseTreeSettingRepository<CostCenter>
{
    public void RemoveChartOfAccounts(List<CostCenterChartOfAccount> chartOfAccounts);
}
