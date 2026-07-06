using Inventory.Application.Commands.Items;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Items;

namespace Inventory.Infrastructure.Workflows;

/// <summary>
/// Item write orchestration. Keeps write path decoupled from handler layer.
/// </summary>
public class ItemWorkflowService(IItemService itemService) : IItemWorkflowService
{
    public Task<ApiResponse<Item>> Create(ItemCreateCommand command)
        => itemService.Create(command);

    public Task<ApiResponse<Item>> Update(ItemUpdateCommand command)
        => itemService.Update(command);

    public Task<ApiResponse<Item>> Delete(Guid id)
        => itemService.Delete(id);
}
