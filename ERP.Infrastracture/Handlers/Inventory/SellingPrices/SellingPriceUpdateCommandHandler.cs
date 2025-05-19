using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.SellingPrices;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;

namespace ERP.Infrastracture.Handlers.Inventory.SellingPrices;

public class SellingPriceUpdateCommandHandler(ISellingPriceService service) : ICommandHandler<SellingPriceUpdateCommand, ApiResponse<SellingPrice>>
{
    public async Task<ApiResponse<SellingPrice>> Handle(SellingPriceUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}