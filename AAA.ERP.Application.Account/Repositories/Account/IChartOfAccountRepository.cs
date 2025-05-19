using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;

namespace ERP.Application.Repositories.Account;

public interface IChartOfAccountRepository : IBaseTreeSettingRepository<ChartOfAccount>
{
    public Task<string> GenerateNewCodeForChild(Guid? parentId);
    public Task<ChartOfAccount?> GetChartOfAccountByCode(string code);

}
