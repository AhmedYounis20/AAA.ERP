using ERP.Application.Data;
using ERP.Application.Repositories;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace ERP.Infrastracture.Repositories.Account;

public class FinancialPeriodUsageQueries(IApplicationDbContext context) : IFinancialPeriodUsageQueries
{
    public Task<bool> IsUsedInInventoryTransactions(Guid financialPeriodId)
        => context.Set<InventoryTransaction>().AnyAsync(e => e.FinancialPeriodId == financialPeriodId);
}
