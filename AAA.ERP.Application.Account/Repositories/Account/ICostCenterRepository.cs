using Domain.Account.Models.Entities.CostCenters;
using ERP.Application.Repositories.BaseRepositories;

namespace ERP.Application.Repositories;

public interface ICostCenterRepository : IBaseTreeSettingRepository<CostCenter>
{
    public void RemoveChartOfAccounts(List<CostCenterChartOfAccount> chartOfAccounts);
}
