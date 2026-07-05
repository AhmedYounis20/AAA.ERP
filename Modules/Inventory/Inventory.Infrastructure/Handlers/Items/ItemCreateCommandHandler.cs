using Inventory.Application.Services;
using Inventory.Application.Commands.Items;
using Inventory.Domain.Models.Entities.Items;

namespace Inventory.Infrastructure.Handlers.Items;

public class ItemCreateCommandHandler(IItemService service) : ICommandHandler<ItemCreateCommand, ApiResponse<Item>>
{
    public async Task<ApiResponse<Item>> Handle(ItemCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}