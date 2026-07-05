using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Domain.Models.Entities.ChartOfAccounts;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for chart of account persistence.
/// </summary>
public interface IChartOfAccountRepository : IBaseTreeSettingRepository<ChartOfAccount>
{
    /// <summary>Generates new code for child.</summary>
    Task<string> GenerateNewCodeForChild(Guid? parentId);
    /// <summary>Gets chart of account by code.</summary>
    Task<ChartOfAccount?> GetChartOfAccountByCode(string code);
}
