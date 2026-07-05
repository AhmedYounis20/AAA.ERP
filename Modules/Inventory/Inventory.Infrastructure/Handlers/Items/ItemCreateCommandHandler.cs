using Inventory.Application.Services;
using Inventory.Application.Commands.Items;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace Inventory.Infrastructure.Handlers.Items;

public class ItemCreateCommandHandler(IItemService service) : ICommandHandler<ItemCreateCommand, ApiResponse<Item>>
{
    public async Task<ApiResponse<Item>> Handle(ItemCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}