using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Inventory.Infrastructure.Repositories;

public class InventoryTransactionRepository : BaseRepository<InventoryTransaction>, IInventoryTransactionRepository
{
    public InventoryTransactionRepository(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<InventoryTransaction?> GetWithItems(Guid id)
    {
        return await _dbSet
            .Include(e => e.Items)
            .ThenInclude(i => i.Variant)
            .Include(e => e.Items)
            .ThenInclude(i => i.PackingUnit)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<InventoryTransaction>> GetByBranch(Guid branchId)
    {
        return await _dbSet
            .Include(e => e.Items)
            .ThenInclude(i => i.Variant)
            .Include(e => e.Items)
            .ThenInclude(i => i.PackingUnit)
            .Where(e => e.BranchId == branchId)
            .OrderByDescending(e => e.TransactionDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryTransaction>> GetByTransactionType(InventoryTransactionType transactionType)
    {
        return await _dbSet
            .Include(e => e.Items)
            .ThenInclude(i => i.Variant)
            .Include(e => e.Items)
            .ThenInclude(i => i.PackingUnit)
            .Where(e => e.TransactionType == transactionType)
            .OrderByDescending(e => e.TransactionDate)
            .ToListAsync();
    }

    public async Task<bool> TransactionAfterThisDateExist(DateTime date)
    {
        return await _dbSet.AnyAsync(e => e.TransactionDate >= date);
    }
}
