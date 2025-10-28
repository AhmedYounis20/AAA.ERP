using Inventory.Domain.Commands.SellingPrices;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Infrastructure.Handlers.SellingPrices;

public class SellingPriceCreateCommandHandler(ISellingPriceService service) : ICommandHandler<SellingPriceCreateCommand, ApiResponse<SellingPrice>>
{
    public async Task<ApiResponse<SellingPrice>> Handle(SellingPriceCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
