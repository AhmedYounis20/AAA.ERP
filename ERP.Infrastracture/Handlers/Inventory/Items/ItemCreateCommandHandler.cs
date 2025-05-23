using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace ERP.Infrastracture.Handlers.Inventory.SellingPrices;

public class ItemCreateCommandHandler(IItemService service) : ICommandHandler<ItemCreateCommand, ApiResponse<Item>>
{
    public async Task<ApiResponse<Item>> Handle(ItemCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}