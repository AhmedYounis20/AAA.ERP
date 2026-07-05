using Inventory.Application.Commands.SellingPrices;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Infrastructure.Handlers.SellingPrices;

public class SellingPriceCreateCommandHandler(ISellingPriceService service)
    : ICommandHandler<SellingPriceCreateCommand, ApiResponse<SellingPrice>>
{
    public Task<ApiResponse<SellingPrice>> Handle(SellingPriceCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
