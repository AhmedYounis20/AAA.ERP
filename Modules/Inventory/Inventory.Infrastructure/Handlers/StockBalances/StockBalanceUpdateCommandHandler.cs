using Inventory.Application.Commands.StockBalances;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Infrastructure.Handlers.StockBalances;

public class StockBalanceUpdateCommandHandler(IStockBalanceService service)
    : ICommandHandler<StockBalanceUpdateCommand, ApiResponse<StockBalance>>
{
    public Task<ApiResponse<StockBalance>> Handle(StockBalanceUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
