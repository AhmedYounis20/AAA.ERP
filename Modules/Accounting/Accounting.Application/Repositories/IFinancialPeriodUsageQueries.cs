namespace Accounting.Application.Repositories;

/// <summary>
/// Contract for financial period usage queries.
/// </summary>
public interface IFinancialPeriodUsageQueries
{
    /// <summary>is used in inventory transactions operation.</summary>
    Task<bool> IsUsedInInventoryTransactions(Guid financialPeriodId);
}
