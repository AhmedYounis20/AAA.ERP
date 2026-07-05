namespace ERP.Application.Repositories;

public interface IFinancialPeriodUsageQueries
{
    Task<bool> IsUsedInInventoryTransactions(Guid financialPeriodId);
}
