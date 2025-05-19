using Domain.Account.Models.Entities.ChartOfAccounts;
using ERP.Application.Repositories.BaseRepositories;

namespace ERP.Application.Repositories;

public interface IChartOfAccountRepository : IBaseTreeSettingRepository<ChartOfAccount>
{
    public Task<string> GenerateNewCodeForChild(Guid? parentId);
    public Task<ChartOfAccount?> GetChartOfAccountByCode(string code);

}
