using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory;
using ERP.Domain.Models.Entities.Inventory;

namespace ERP.Infrastracture.Handlers.Inventory.InventoryTransactions;

public class InventoryTransferCreateCommandHandler : ICommandHandler<InventoryTransferCreateCommand, ApiResponse<InventoryTransfer>>
{
    private readonly IInventoryTransferService _service;
    public InventoryTransferCreateCommandHandler(IInventoryTransferService service)
    {
        _service = service;
    }
    public async Task<ApiResponse<InventoryTransfer>> Handle(InventoryTransferCreateCommand request, CancellationToken cancellationToken)
    {
        return await _service.Create(request);
    }
} 