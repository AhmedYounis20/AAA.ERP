using Inventory.Domain.Commands;
using Inventory.Domain.Models.Entities;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class InventoryTransferUpdateCommandHandler : ICommandHandler<InventoryTransferUpdateCommand, ApiResponse<InventoryTransfer>>
{
    private readonly IInventoryTransferService _service;
    public InventoryTransferUpdateCommandHandler(IInventoryTransferService service)
    {
        _service = service;
    }
    public async Task<ApiResponse<InventoryTransfer>> Handle(InventoryTransferUpdateCommand request, CancellationToken cancellationToken)
    {
        return await _service.Update(request);
    }
} 
