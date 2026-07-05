using Inventory.Application.Commands.StockBalances;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Infrastructure.Handlers.StockBalances;

public class StockBalanceCreateCommandHandler(IStockBalanceService service)
    : ICommandHandler<StockBalanceCreateCommand, ApiResponse<StockBalance>>
{
    public Task<ApiResponse<StockBalance>> Handle(StockBalanceCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
