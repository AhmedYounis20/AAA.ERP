namespace Accounting.Application.Repositories;

public interface IFinancialPeriodUsageQueries
{
    Task<bool> IsUsedInInventoryTransactions(Guid financialPeriodId);
}
