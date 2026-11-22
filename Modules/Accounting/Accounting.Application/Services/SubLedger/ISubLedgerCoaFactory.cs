using Accounting.Domain.Models.Entities.ChartOfAccounts;

namespace Accounting.Application.Services;

/// <summary>
/// Creates chart-of-account nodes for sub-ledger entities under a resolved parent.
/// </summary>
public interface ISubLedgerCoaFactory
{
    Task<ChartOfAccount> CreateChildAccountAsync(
        string name,
        string? nameSecondLanguage,
        ChartOfAccount parent,
        SubLeadgerType subLedgerType,
        CancellationToken cancellationToken = default);

    Task<ChartOfAccount> CreatePrefixedChildAccountAsync(
        ChartOfAccount namePrefixSource,
        string entityName,
        string? entityNameSecondLanguage,
        ChartOfAccount parent,
        CancellationToken cancellationToken = default);
}
