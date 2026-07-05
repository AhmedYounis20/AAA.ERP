using Inventory.Application.Commands.InventoryTransfers;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities;

namespace Inventory.Infrastructure.Handlers.InventoryTransfers;

public class InventoryTransferCreateCommandHandler(IInventoryTransferService service)
    : ICommandHandler<InventoryTransferCreateCommand, ApiResponse<InventoryTransfer>>
{
    public Task<ApiResponse<InventoryTransfer>> Handle(InventoryTransferCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
