using Inventory.Domain.Commands.SellingPrices;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Infrastructure.Handlers.SellingPrices;

public class SellingPriceUpdateCommandHandler(ISellingPriceService service) : ICommandHandler<SellingPriceUpdateCommand, ApiResponse<SellingPrice>>
{
    public async Task<ApiResponse<SellingPrice>> Handle(SellingPriceUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}