using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory;
using ERP.Domain.Models.Entities.Inventory;

namespace ERP.Infrastracture.Handlers.Inventory.InventoryTransactions;

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