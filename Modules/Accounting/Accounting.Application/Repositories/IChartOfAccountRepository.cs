using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Domain.Models.Entities.ChartOfAccounts;

namespace Accounting.Application.Repositories;

public interface IChartOfAccountRepository : IBaseTreeSettingRepository<ChartOfAccount>
{
    Task<string> GenerateNewCodeForChild(Guid? parentId);
    Task<ChartOfAccount?> GetChartOfAccountByCode(string code);
}
