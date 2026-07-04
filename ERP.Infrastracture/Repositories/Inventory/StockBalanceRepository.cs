using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Inventory;

public class StockBalanceRepository : BaseRepository<StockBalance>, IStockBalanceRepository
{
    public StockBalanceRepository(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<StockBalance?> GetByVariantAndBranch(Guid variantId, Guid branchId)
    {
        return await _dbSet
            .Include(e => e.Variant)
            .Include(e => e.PackingUnit)
            .Include(e => e.Branch)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.VariantId == variantId && e.BranchId == branchId);
    }

    public async Task<StockBalance?> GetByVariantPackingUnitAndBranch(Guid variantId, Guid packingUnitId, Guid branchId)
    {
        return await _dbSet
            .Include(e => e.Variant)
            .Include(e => e.PackingUnit)
            .Include(e => e.Branch)
            .FirstOrDefaultAsync(e => e.VariantId == variantId && e.PackingUnitId == packingUnitId && e.BranchId == branchId);
    }
    public async Task<StockBalance?> GetByVariantPackingUnitAndBranchWithoutInclues(Guid variantId, Guid packingUnitId, Guid branchId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(e => e.VariantId == variantId && e.PackingUnitId == packingUnitId && e.BranchId == branchId);
    }

    public async Task<IEnumerable<StockBalance>> GetByBranch(Guid branchId)
    {
        return await _dbSet
            .Include(e => e.Variant)
                .ThenInclude(v => v.Product)
            .Include(e => e.PackingUnit)
            .Include(e => e.Branch)
            .AsNoTracking()
            .Where(e => e.BranchId == branchId)
            .OrderBy(e => e.Variant.Product != null ? e.Variant.Product.Name : string.Empty)
            .ToListAsync();
    }

    public async Task<IEnumerable<StockBalance>> GetByVariant(Guid variantId)
    {
        return await _dbSet
            .Include(e => e.Variant)
            .Include(e => e.PackingUnit)
            .Include(e => e.Branch)
            .AsNoTracking()
            .Where(e => e.VariantId == variantId)
            .OrderBy(e => e.Branch.Name)
            .ToListAsync();
    }

    public async Task<decimal> GetCurrentBalance(Guid variantId, Guid packingUnitId, Guid branchId)
    {
        var stockBalance = await _dbSet
            .Where(e => e.VariantId == variantId && e.PackingUnitId == packingUnitId && e.BranchId == branchId)
            .FirstOrDefaultAsync();

        return stockBalance?.CurrentBalance ?? 0;
    }
} 