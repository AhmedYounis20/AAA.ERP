using ERP.Application.Services.BaseServices;
using Inventory.Application.Commands.StockBalances;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace Inventory.Application.Services;

public interface IStockBalanceService : IBaseService<StockBalance, StockBalanceCreateCommand, StockBalanceUpdateCommand>
{
    Task<ApiResponse<StockBalance>> GetByVariantAndBranch(Guid variantId, Guid branchId);
    Task<ApiResponse<StockBalance>> GetByVariantPackingUnitAndBranch(Guid variantId, Guid packingUnitId, Guid branchId);
    Task<ApiResponse<IEnumerable<StockBalance>>> GetByBranch(Guid branchId);
    Task<ApiResponse<IEnumerable<StockBalance>>> GetByVariant(Guid variantId);
    Task<ApiResponse<decimal>> GetCurrentBalance(Guid variantId, Guid packingUnitId, Guid branchId);
    Task<ApiResponse<bool>> UpdateStockBalance(Guid variantId, Guid packingUnitId, Guid branchId, decimal quantity, decimal unitCost, bool isReceipt);
}
