using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.InventoryTransactions;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace ERP.Infrastracture.Handlers.Inventory.InventoryTransactions;

public class InventoryTransactionUpdateCommandHandler(IInventoryTransactionService service) : ICommandHandler<InventoryTransactionUpdateCommand, ApiResponse<InventoryTransaction>>
{
    public async Task<ApiResponse<InventoryTransaction>> Handle(InventoryTransactionUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
} 