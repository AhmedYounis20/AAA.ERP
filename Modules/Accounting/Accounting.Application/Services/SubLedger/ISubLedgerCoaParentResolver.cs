using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Services;

/// <summary>
/// Resolves sub-ledger base chart-of-account parents by domain type (never by hardcoded ID).
/// </summary>
public interface ISubLedgerCoaParentResolver
{
    Task<ChartOfAccount> ResolveAsync(
        SubLeadgerType subLedgerType,
        FixedAssetType? fixedAssetType = null,
        SubLedgerAccountRole role = SubLedgerAccountRole.Default,
        CancellationToken cancellationToken = default);
}
