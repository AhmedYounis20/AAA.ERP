using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.StockBalances;

namespace Inventory.Application.Repositories;

/// <summary>
/// Repository contract for stock balance persistence.
/// </summary>
public interface IStockBalanceRepository : IBaseRepository<StockBalance>
{
    /// <summary>Gets by variant and branch.</summary>
    Task<StockBalance?> GetByVariantAndBranch(Guid variantId, Guid branchId);
    /// <summary>Gets by variant packing unit and branch.</summary>
    Task<StockBalance?> GetByVariantPackingUnitAndBranch(Guid variantId, Guid packingUnitId, Guid branchId);
    Task<IEnumerable<StockBalance>> GetByBranch(Guid branchId);
    Task<IEnumerable<StockBalance>> GetByVariant(Guid variantId);
    /// <summary>Gets current balance.</summary>
    Task<decimal> GetCurrentBalance(Guid variantId, Guid packingUnitId, Guid branchId);
    /// <summary>Gets by variant packing unit and branch without inclues.</summary>
    Task<StockBalance?> GetByVariantPackingUnitAndBranchWithoutInclues(Guid variantId, Guid packingUnitId, Guid branchId);

}
