using Inventory.Application.Commands.StockBalances;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Infrastructure.Handlers.StockBalances;

public class StockBalanceDeleteCommandHandler(IStockBalanceService stockBalanceService)
    : ICommandHandler<StockBalanceDeleteCommand, ApiResponse<StockBalance>>
{
    public Task<ApiResponse<StockBalance>> Handle(StockBalanceDeleteCommand request, CancellationToken cancellationToken)
        => stockBalanceService.Delete(request.Id);
}
