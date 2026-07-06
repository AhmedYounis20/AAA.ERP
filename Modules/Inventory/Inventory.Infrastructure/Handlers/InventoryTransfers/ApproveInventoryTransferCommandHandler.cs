using Inventory.Application.Commands.InventoryTransfers;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities;

namespace Inventory.Infrastructure.Handlers.InventoryTransfers;

public sealed class ApproveInventoryTransferCommandHandler(IInventoryTransferWorkflowService workflow)
    : ICommandHandler<ApproveInventoryTransferCommand, ApiResponse<InventoryTransfer>>
{
    public Task<ApiResponse<InventoryTransfer>> Handle(
        ApproveInventoryTransferCommand request,
        CancellationToken cancellationToken)
        => workflow.ApproveTransfer(request.Id);
}

public sealed class RejectInventoryTransferCommandHandler(IInventoryTransferWorkflowService workflow)
    : ICommandHandler<RejectInventoryTransferCommand, ApiResponse<InventoryTransfer>>
{
    public Task<ApiResponse<InventoryTransfer>> Handle(
        RejectInventoryTransferCommand request,
        CancellationToken cancellationToken)
        => workflow.RejectTransfer(request.Id, request.ApproverId, request.Reason);
}
