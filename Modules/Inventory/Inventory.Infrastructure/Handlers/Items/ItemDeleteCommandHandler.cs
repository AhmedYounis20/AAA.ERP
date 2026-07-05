using Inventory.Application.Commands.Items;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Items;

namespace Inventory.Infrastructure.Handlers.Items;

public class ItemDeleteCommandHandler(IItemService itemService)
    : ICommandHandler<ItemDeleteCommand, ApiResponse<Item>>
{
    public Task<ApiResponse<Item>> Handle(ItemDeleteCommand request, CancellationToken cancellationToken)
        => itemService.Delete(request.Id);
}
