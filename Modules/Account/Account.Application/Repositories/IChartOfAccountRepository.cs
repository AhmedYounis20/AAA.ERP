using Account.Domain.Models.Entities.ChartOfAccounts;
using Shared.Application.Repositories;

namespace Account.Application.Repositories;

public interface IChartOfAccountRepository : IBaseTreeSettingRepository<ChartOfAccount>
{
    public Task<string> GenerateNewCodeForChild(Guid? parentId);
    public Task<ChartOfAccount?> GetChartOfAccountByCode(string code);

}
