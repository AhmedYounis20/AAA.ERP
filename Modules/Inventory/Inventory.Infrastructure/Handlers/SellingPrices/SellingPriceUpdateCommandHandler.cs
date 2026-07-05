using Inventory.Application.Commands.SellingPrices;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;

namespace Inventory.Infrastructure.Handlers.SellingPrices;

public class SellingPriceUpdateCommandHandler(ISellingPriceService service)
    : ICommandHandler<SellingPriceUpdateCommand, ApiResponse<SellingPrice>>
{
    public Task<ApiResponse<SellingPrice>> Handle(SellingPriceUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
