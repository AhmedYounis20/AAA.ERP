using Inventory.Application.Commands.SellingPrices;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Infrastructure.Handlers.SellingPrices;

public class SellingPriceUpdateCommandHandler(ISellingPriceService service)
    : ICommandHandler<SellingPriceUpdateCommand, ApiResponse<SellingPrice>>
{
    public Task<ApiResponse<SellingPrice>> Handle(SellingPriceUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
