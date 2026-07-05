using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.Sizes;
using ERP.Infrastructure.Repositories.BaseRepositories;

namespace Inventory.Infrastructure.Repositories;

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
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.VariantId == variantId && e.BranchId == branchId);
    }

    public async Task<StockBalance?> GetByVariantPackingUnitAndBranch(Guid variantId, Guid packingUnitId, Guid branchId)
    {
        return await _dbSet
            .Include(e => e.Variant)
            .Include(e => e.PackingUnit)
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
            .AsNoTracking()
            .Where(e => e.VariantId == variantId)
            .OrderBy(e => e.BranchId)
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
