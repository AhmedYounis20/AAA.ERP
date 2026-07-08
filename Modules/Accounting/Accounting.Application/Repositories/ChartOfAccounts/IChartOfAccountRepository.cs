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
    /// <summary>Checks whether the account is referenced by journal entry lines.</summary>
    Task<bool> IsUsedInEntries(Guid id);
    /// <summary>Checks whether the account is linked to cost centers.</summary>
    Task<bool> IsUsedInCostCenters(Guid id);
    /// <summary>Checks whether the account is linked to sub-ledgers.</summary>
    Task<bool> IsUsedInSubLedgers(Guid id);
    /// <summary>Checks whether the account is referenced anywhere.</summary>
    Task<bool> IsInUse(Guid id);
}
