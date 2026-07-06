using Inventory.Application.Services;
using Inventory.Application.Commands.Items;
using Inventory.Domain.Models.Entities.Items;

namespace Inventory.Infrastructure.Handlers.Items;

public sealed class ItemCreateCommandHandler(IItemWorkflowService workflow)
    : ICommandHandler<ItemCreateCommand, ApiResponse<Item>>
{
    public Task<ApiResponse<Item>> Handle(ItemCreateCommand request, CancellationToken cancellationToken)
        => workflow.Create(request);
}

public sealed class ItemUpdateCommandHandler(IItemWorkflowService workflow)
    : ICommandHandler<ItemUpdateCommand, ApiResponse<Item>>
{
    public Task<ApiResponse<Item>> Handle(ItemUpdateCommand request, CancellationToken cancellationToken)
        => workflow.Update(request);
}

public sealed class ItemDeleteCommandHandler(IItemWorkflowService workflow)
    : ICommandHandler<ItemDeleteCommand, ApiResponse<Item>>
{
    public Task<ApiResponse<Item>> Handle(ItemDeleteCommand request, CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}