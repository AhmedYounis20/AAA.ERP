using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Inventory;

public class InventoryTransactionRepository : BaseRepository<InventoryTransaction>, IInventoryTransactionRepository
{
    public InventoryTransactionRepository(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<InventoryTransaction?> GetWithItems(Guid id)
    {
        return await _dbSet
            .Include(e => e.Items)
            .ThenInclude(i => i.Item)
            .Include(e => e.Items)
            .ThenInclude(i => i.PackingUnit)
            .Include(e => e.TransactionParty)
            .Include(e => e.Branch)
            .Include(e => e.FinancialPeriod)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<InventoryTransaction>> GetByBranch(Guid branchId)
    {
        return await _dbSet
            .Include(e => e.Items)
            .ThenInclude(i => i.Item)
            .Include(e => e.Items)
            .ThenInclude(i => i.PackingUnit)
            .Include(e => e.TransactionParty)
            .Include(e => e.Branch)
            .Where(e => e.BranchId == branchId)
            .OrderByDescending(e => e.TransactionDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryTransaction>> GetByTransactionType(InventoryTransactionType transactionType)
    {
        return await _dbSet
            .Include(e => e.Items)
            .ThenInclude(i => i.Item)
            .Include(e => e.Items)
            .ThenInclude(i => i.PackingUnit)
            .Include(e => e.TransactionParty)
            .Include(e => e.Branch)
            .Where(e => e.TransactionType == transactionType)
            .OrderByDescending(e => e.TransactionDate)
            .ToListAsync();
    }
    
    public  async Task<bool> TransactionAfterThisDateExist(DateTime date)
    {
        return await _dbSet.AnyAsync(e=>e.TransactionDate >= date);
    }
} 