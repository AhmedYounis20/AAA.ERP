using Inventory.Application.Commands.InventoryTransfers;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities;

namespace Inventory.Infrastructure.Handlers.InventoryTransfers;

public class ApproveInventoryTransferCommandHandler(IInventoryTransferService transferService)
    : ICommandHandler<ApproveInventoryTransferCommand, ApiResponse<InventoryTransfer>>
{
    public Task<ApiResponse<InventoryTransfer>> Handle(
        ApproveInventoryTransferCommand request,
        CancellationToken cancellationToken)
        => transferService.ApproveTransfer(request.Id);
}
