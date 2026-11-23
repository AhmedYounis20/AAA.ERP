using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.StockBalances;

namespace Inventory.Application.Repositories;

/// <summary>
/// Repository contract for stock balance persistence.
/// </summary>
public interface IStockBalanceRepository : IBaseRepository<StockBalance>
{
    /// <summary>Gets all balances with variant/product/packing includes for display.</summary>
    Task<IEnumerable<StockBalance>> GetAllWithDetails();
    /// <summary>Gets one balance with display includes.</summary>
    Task<StockBalance?> GetByIdWithDetails(Guid id);
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
