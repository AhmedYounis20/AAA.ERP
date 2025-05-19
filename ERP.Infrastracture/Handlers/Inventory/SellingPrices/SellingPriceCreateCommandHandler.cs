using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.SellingPrices;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;

namespace ERP.Infrastracture.Handlers.Inventory.SellingPrices;

public class SellingPriceCreateCommandHandler(ISellingPriceService service) : ICommandHandler<SellingPriceCreateCommand, ApiResponse<SellingPrice>>
{
    public async Task<ApiResponse<SellingPrice>> Handle(SellingPriceCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}