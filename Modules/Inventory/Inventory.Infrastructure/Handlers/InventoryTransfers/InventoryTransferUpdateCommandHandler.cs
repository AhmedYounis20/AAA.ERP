using Inventory.Application.Commands.InventoryTransfers;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory;

namespace Inventory.Infrastructure.Handlers.InventoryTransfers;

public class InventoryTransferUpdateCommandHandler(IInventoryTransferService service)
    : ICommandHandler<InventoryTransferUpdateCommand, ApiResponse<InventoryTransfer>>
{
    public Task<ApiResponse<InventoryTransfer>> Handle(InventoryTransferUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
