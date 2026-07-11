using Accounting.Contracts.Integration;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Infrastructure.Services;

public class InventoryAccountingDisplayQueries(IApplicationDbContext context) : IInventoryAccountingDisplayQueries
{
    public async Task<IReadOnlyDictionary<Guid, AccountingEntityDisplayInfo>> GetBranchesByIdsAsync(
        IReadOnlyCollection<Guid> branchIds,
        CancellationToken cancellationToken = default)
    {
        if (branchIds.Count == 0)
            return new Dictionary<Guid, AccountingEntityDisplayInfo>();

        var rows = await context.Set<Branch>()
            .AsNoTracking()
            .Include(b => b.ChartOfAccount)
            .Where(b => branchIds.Contains(b.Id))
            .Select(b => new AccountingEntityDisplayInfo(
                b.Id,
                b.Name,
                b.NameSecondLanguage,
                b.ChartOfAccount != null ? b.ChartOfAccount.Code : null))
            .ToListAsync(cancellationToken);

        return rows.ToDictionary(r => r.Id);
    }

    public async Task<IReadOnlyDictionary<Guid, AccountingEntityDisplayInfo>> GetPartyAccountsByIdsAsync(
        IReadOnlyCollection<Guid> chartOfAccountIds,
        CancellationToken cancellationToken = default)
    {
        if (chartOfAccountIds.Count == 0)
            return new Dictionary<Guid, AccountingEntityDisplayInfo>();

        var rows = await context.Set<ChartOfAccount>()
            .AsNoTracking()
            .Where(c => chartOfAccountIds.Contains(c.Id))
            .Select(c => new AccountingEntityDisplayInfo(
                c.Id,
                c.Name,
                c.NameSecondLanguage,
                c.Code))
            .ToListAsync(cancellationToken);

        return rows.ToDictionary(r => r.Id);
    }
}
