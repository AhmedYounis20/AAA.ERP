using Accounting.Application.Repositories;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Accounting.Infrastructure.Repositories;

public class FinancialPeriodUsageQueries(IApplicationDbContext context) : IFinancialPeriodUsageQueries
{
    public Task<bool> IsUsedInInventoryTransactions(Guid financialPeriodId)
        => context.Set<InventoryTransaction>().AnyAsync(e => e.FinancialPeriodId == financialPeriodId);
}
