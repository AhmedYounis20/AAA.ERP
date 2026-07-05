using Inventory.Application.Commands.InventoryTransfers;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities;

namespace Inventory.Infrastructure.Handlers.InventoryTransfers;

public class RejectInventoryTransferCommandHandler(IInventoryTransferService transferService)
    : ICommandHandler<RejectInventoryTransferCommand, ApiResponse<InventoryTransfer>>
{
    public Task<ApiResponse<InventoryTransfer>> Handle(
        RejectInventoryTransferCommand request,
        CancellationToken cancellationToken)
        => transferService.RejectTransfer(request.Id, request.ApproverId, request.Reason);
}
