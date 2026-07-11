using Inventory.Application.Dtos.Catalog;
using Inventory.Domain.Models.Entities.StockBalances;

namespace Inventory.Application.Services;

/// <summary>
/// Stock balance domain operations (reads, balance updates). CRUD writes are handled by CQRS handlers.
/// </summary>
public interface IStockBalanceService
{
    Task<ApiResponse<StockBalanceReadDto>> GetDisplayById(Guid id);
    Task<ApiResponse<IEnumerable<StockBalanceReadDto>>> GetAllForDisplay();
    Task<ApiResponse<StockBalanceReadDto>> GetByVariantAndBranch(Guid variantId, Guid branchId);
    Task<ApiResponse<StockBalanceReadDto>> GetByVariantPackingUnitAndBranch(Guid variantId, Guid packingUnitId, Guid branchId);
    Task<ApiResponse<IEnumerable<StockBalanceReadDto>>> GetByBranch(Guid branchId);
    Task<ApiResponse<IEnumerable<StockBalanceReadDto>>> GetByVariant(Guid variantId);
    Task<ApiResponse<decimal>> GetCurrentBalance(Guid variantId, Guid packingUnitId, Guid branchId);
    Task<ApiResponse<bool>> UpdateStockBalance(Guid variantId, Guid packingUnitId, Guid branchId, decimal quantity, decimal unitCost, bool isReceipt);
}
