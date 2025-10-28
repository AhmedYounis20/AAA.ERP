using Inventory.Domain.Commands.Items;

namespace Inventory.Infrastructure.Handlers.Items;

public class ItemUpdateCommandHandler(IItemService service) : ICommandHandler<ItemUpdateCommand, ApiResponse<Item>>
{
    public async Task<ApiResponse<Item>> Handle(ItemUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
