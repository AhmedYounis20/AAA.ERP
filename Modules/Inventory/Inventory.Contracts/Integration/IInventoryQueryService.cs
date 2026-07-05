namespace Inventory.Contracts.Integration;

/// <summary>
/// Read-only inventory queries consumed by other modules (accounting, reporting).
/// </summary>
public interface IInventoryQueryService
{
    Task<bool> IsUsedInInventoryTransactionsAsync(Guid financialPeriodId, CancellationToken cancellationToken = default);

    Task<bool> TransactionExistsAfterDateAsync(DateTime date, CancellationToken cancellationToken = default);
}
