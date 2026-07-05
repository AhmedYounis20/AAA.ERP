using Inventory.Application.Commands.StockBalances;
using Inventory.Application.Services;

namespace Inventory.Infrastructure.Handlers.StockBalances;

public class UpdateStockBalanceCommandHandler(IStockBalanceService stockBalanceService)
    : ICommandHandler<UpdateStockBalanceCommand, ApiResponse<bool>>
{
    public Task<ApiResponse<bool>> Handle(UpdateStockBalanceCommand request, CancellationToken cancellationToken)
        => stockBalanceService.UpdateStockBalance(
            request.VariantId,
            request.PackingUnitId,
            request.BranchId,
            request.Quantity,
            request.UnitCost,
            request.IsReceipt);
}
