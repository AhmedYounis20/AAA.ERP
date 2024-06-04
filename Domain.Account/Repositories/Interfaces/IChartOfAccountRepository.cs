using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Repositories.BaseRepositories.Interfaces;

namespace Domain.Account.Repositories.Interfaces;

public interface IChartOfAccountRepository: IBaseTreeSettingRepository<ChartOfAccount> {
    public Task<string> GenerateNewCodeForChild(Guid? parentId);
    public Task<ChartOfAccount?> GetChartOfAccountByCode(string code);

}
