using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Repositories;

public sealed class SubLedgerBaseAccountRepository(IApplicationDbContext context) : ISubLedgerBaseAccountRepository
{
    public async Task<IReadOnlyList<SubLedgerBaseAccount>> FindAsync(
        SubLeadgerType subLedgerType,
        SubLedgerAccountRole role,
        FixedAssetType? fixedAssetType,
        CancellationToken cancellationToken = default)
    {
        return await context.Set<SubLedgerBaseAccount>()
            .Include(e => e.ChartOfAccount)
            .Where(e => e.SubLedgerType == subLedgerType
                        && e.Role == role
                        && e.FixedAssetType == fixedAssetType)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SubLedgerBaseAccount>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await context.Set<SubLedgerBaseAccount>()
            .Include(e => e.ChartOfAccount)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsForAccountAsync(Guid chartOfAccountId, CancellationToken cancellationToken = default)
        => context.Set<SubLedgerBaseAccount>()
            .AnyAsync(e => e.ChartOfAccountId == chartOfAccountId, cancellationToken);
}
