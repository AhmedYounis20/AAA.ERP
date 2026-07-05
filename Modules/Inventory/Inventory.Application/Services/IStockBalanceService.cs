using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.StockBalances;
using Inventory.Domain.Models.Dtos.Inventory;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Application.Services;

/// <summary>
/// Application service contract for stock balance.
/// </summary>
public interface IStockBalanceService : IBaseService<StockBalance, StockBalanceCreateCommand, StockBalanceUpdateCommand>
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
