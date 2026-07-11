using Inventory.Application.Commands.InventoryTransfers;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities;

namespace Inventory.Infrastructure.Handlers.InventoryTransfers;

public sealed class InventoryTransferCreateCommandHandler(IInventoryTransferWorkflowService workflow)
    : ICommandHandler<InventoryTransferCreateCommand, ApiResponse<InventoryTransfer>>
{
    public Task<ApiResponse<InventoryTransfer>> Handle(
        InventoryTransferCreateCommand request,
        CancellationToken cancellationToken)
        => workflow.Create(request);
}

public sealed class InventoryTransferUpdateCommandHandler(IInventoryTransferWorkflowService workflow)
    : ICommandHandler<InventoryTransferUpdateCommand, ApiResponse<InventoryTransfer>>
{
    public Task<ApiResponse<InventoryTransfer>> Handle(
        InventoryTransferUpdateCommand request,
        CancellationToken cancellationToken)
        => workflow.Update(request);
}
