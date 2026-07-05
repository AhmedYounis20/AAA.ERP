using Inventory.Application.Commands.SellingPrices;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;

namespace Inventory.Infrastructure.Handlers.SellingPrices;

public class SellingPriceCreateCommandHandler(ISellingPriceService service)
    : ICommandHandler<SellingPriceCreateCommand, ApiResponse<SellingPrice>>
{
    public Task<ApiResponse<SellingPrice>> Handle(SellingPriceCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
