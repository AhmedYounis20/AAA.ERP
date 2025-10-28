using Inventory.Domain.Commands;
using Inventory.Domain.Models.Entities;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

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
