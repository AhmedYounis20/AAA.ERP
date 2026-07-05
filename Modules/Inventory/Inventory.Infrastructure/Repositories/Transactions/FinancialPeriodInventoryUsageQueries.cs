using Inventory.Contracts.Integration;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Repositories;

public class FinancialPeriodInventoryUsageQueries(IApplicationDbContext context)
    : IFinancialPeriodInventoryUsageQueries, IInventoryQueryService
{
    public Task<bool> IsUsedInInventoryTransactionsAsync(
        Guid financialPeriodId,
        CancellationToken cancellationToken = default)
        => context.Set<InventoryTransaction>()
            .AnyAsync(e => e.FinancialPeriodId == financialPeriodId, cancellationToken);

    public Task<bool> TransactionExistsAfterDateAsync(
        DateTime date,
        CancellationToken cancellationToken = default)
        => context.Set<InventoryTransaction>()
            .AnyAsync(e => e.TransactionDate > date, cancellationToken);
}
