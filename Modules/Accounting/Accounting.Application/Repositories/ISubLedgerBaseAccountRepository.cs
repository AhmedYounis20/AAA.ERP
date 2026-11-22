using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Repositories;

/// <summary>
/// Persistence for the sub-ledger posting-parent binding.
/// </summary>
public interface ISubLedgerBaseAccountRepository
{
    Task<IReadOnlyList<SubLedgerBaseAccount>> FindAsync(
        SubLeadgerType subLedgerType,
        SubLedgerAccountRole role,
        FixedAssetType? fixedAssetType,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SubLedgerBaseAccount>> ListAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsForAccountAsync(Guid chartOfAccountId, CancellationToken cancellationToken = default);
}
