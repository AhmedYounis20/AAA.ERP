using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Application.Repositories;

public interface IStockBalanceRepository : IBaseRepository<StockBalance>
{
    Task<StockBalance?> GetByVariantAndBranch(Guid variantId, Guid branchId);
    Task<StockBalance?> GetByVariantPackingUnitAndBranch(Guid variantId, Guid packingUnitId, Guid branchId);
    Task<IEnumerable<StockBalance>> GetByBranch(Guid branchId);
    Task<IEnumerable<StockBalance>> GetByVariant(Guid variantId);
    Task<decimal> GetCurrentBalance(Guid variantId, Guid packingUnitId, Guid branchId);
    Task<StockBalance?> GetByVariantPackingUnitAndBranchWithoutInclues(Guid variantId, Guid packingUnitId, Guid branchId);

} 