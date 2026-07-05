using Inventory.Application.Commands.StockBalances;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace Inventory.Infrastructure.Handlers.StockBalances;

public class StockBalanceUpdateCommandHandler(IStockBalanceService service)
    : ICommandHandler<StockBalanceUpdateCommand, ApiResponse<StockBalance>>
{
    public Task<ApiResponse<StockBalance>> Handle(StockBalanceUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
