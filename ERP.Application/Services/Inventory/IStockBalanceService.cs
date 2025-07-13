using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Inventory.StockBalances;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using Shared.Responses;

namespace ERP.Application.Services.Inventory;

public interface IStockBalanceService : IBaseService<StockBalance, StockBalanceCreateCommand, StockBalanceUpdateCommand>
{
    Task<ApiResponse<StockBalance>> GetByItemAndBranch(Guid itemId, Guid branchId);
    Task<ApiResponse<StockBalance>> GetByItemPackingUnitAndBranch(Guid itemId, Guid packingUnitId, Guid branchId);
    Task<ApiResponse<IEnumerable<StockBalance>>> GetByBranch(Guid branchId);
    Task<ApiResponse<IEnumerable<StockBalance>>> GetByItem(Guid itemId);
    Task<ApiResponse<decimal>> GetCurrentBalance(Guid itemId, Guid packingUnitId, Guid branchId);
    Task<ApiResponse<bool>> UpdateStockBalance(Guid itemId, Guid packingUnitId, Guid branchId, decimal quantity, decimal unitCost, bool isReceipt);
} 