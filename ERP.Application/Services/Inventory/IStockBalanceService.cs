using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Inventory.StockBalances;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using Shared.Responses;

namespace ERP.Application.Services.Inventory;

public interface IStockBalanceService : IBaseService<StockBalance, StockBalanceCreateCommand, StockBalanceUpdateCommand>
{
    Task<ApiResponse<StockBalance>> GetByVariantAndBranch(Guid variantId, Guid branchId);
    Task<ApiResponse<StockBalance>> GetByVariantPackingUnitAndBranch(Guid variantId, Guid packingUnitId, Guid branchId);
    Task<ApiResponse<IEnumerable<StockBalance>>> GetByBranch(Guid branchId);
    Task<ApiResponse<IEnumerable<StockBalance>>> GetByVariant(Guid variantId);
    Task<ApiResponse<decimal>> GetCurrentBalance(Guid variantId, Guid packingUnitId, Guid branchId);
    Task<ApiResponse<bool>> UpdateStockBalance(Guid variantId, Guid packingUnitId, Guid branchId, decimal quantity, decimal unitCost, bool isReceipt);
} 