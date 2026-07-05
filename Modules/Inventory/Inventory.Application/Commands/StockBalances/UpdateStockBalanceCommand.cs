using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Inventory.Application.Commands.StockBalances;

/// <summary>Updates stock quantity and unit cost for a variant at a branch.</summary>
public record UpdateStockBalanceCommand(
    Guid VariantId,
    Guid PackingUnitId,
    Guid BranchId,
    decimal Quantity,
    decimal UnitCost,
    bool IsReceipt) : ICommand<ApiResponse<bool>>;
