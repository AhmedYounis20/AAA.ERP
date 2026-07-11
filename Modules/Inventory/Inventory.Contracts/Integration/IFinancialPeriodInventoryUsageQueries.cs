namespace Inventory.Contracts.Integration;

/// <summary>
/// Cross-module queries for Accounting financial-period validation against inventory data.
/// </summary>
public interface IFinancialPeriodInventoryUsageQueries
{
    /// <summary>Returns true when any inventory transaction references the financial period.</summary>
    Task<bool> IsUsedInInventoryTransactionsAsync(Guid financialPeriodId, CancellationToken cancellationToken = default);

    /// <summary>Returns true when any inventory transaction exists after the given date.</summary>
    Task<bool> TransactionExistsAfterDateAsync(DateTime date, CancellationToken cancellationToken = default);
}
