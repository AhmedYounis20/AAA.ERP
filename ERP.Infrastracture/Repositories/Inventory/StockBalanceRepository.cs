using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Inventory;

public class StockBalanceRepository : BaseRepository<StockBalance>, IStockBalanceRepository
{
    public StockBalanceRepository(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<StockBalance?> GetByItemAndBranch(Guid itemId, Guid branchId)
    {
        return await _dbSet
            .Include(e => e.Item)
            .Include(e => e.PackingUnit)
            .Include(e => e.Branch)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ItemId == itemId && e.BranchId == branchId);
    }

    public async Task<StockBalance?> GetByItemPackingUnitAndBranch(Guid itemId, Guid packingUnitId, Guid branchId)
    {
        return await _dbSet
            .Include(e => e.Item)
            .Include(e => e.PackingUnit)
            .Include(e => e.Branch)
            .FirstOrDefaultAsync(e => e.ItemId == itemId && e.PackingUnitId == packingUnitId && e.BranchId == branchId);
    }
    public async Task<StockBalance?> GetByItemPackingUnitAndBranchWithoutInclues(Guid itemId, Guid packingUnitId, Guid branchId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(e => e.ItemId == itemId && e.PackingUnitId == packingUnitId && e.BranchId == branchId);
    }

    public async Task<IEnumerable<StockBalance>> GetByBranch(Guid branchId)
    {
        return await _dbSet
            .Include(e => e.Item)
            .Include(e => e.PackingUnit)
            .Include(e => e.Branch)
            .AsNoTracking()
            .Where(e => e.BranchId == branchId)
            .OrderBy(e => e.Item.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<StockBalance>> GetByItem(Guid itemId)
    {
        return await _dbSet
            .Include(e => e.Item)
            .Include(e => e.PackingUnit)
            .Include(e => e.Branch)
            .AsNoTracking()
            .Where(e => e.ItemId == itemId)
            .OrderBy(e => e.Branch.Name)
            .ToListAsync();
    }

    public async Task<decimal> GetCurrentBalance(Guid itemId, Guid packingUnitId, Guid branchId)
    {
        var stockBalance = await _dbSet
            .Where(e => e.ItemId == itemId && e.PackingUnitId == packingUnitId && e.BranchId == branchId)
            .FirstOrDefaultAsync();

        return stockBalance?.CurrentBalance ?? 0;
    }
} 