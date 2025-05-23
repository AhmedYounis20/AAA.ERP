using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace ERP.Infrastracture.Handlers.Inventory.Items;

public class ItemUpdateCommandHandler(IItemService service) : ICommandHandler<ItemUpdateCommand, ApiResponse<Item>>
{
    public async Task<ApiResponse<Item>> Handle(ItemUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}