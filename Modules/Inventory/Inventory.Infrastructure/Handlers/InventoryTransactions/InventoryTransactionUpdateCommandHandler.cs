using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class InventoryTransactionUpdateCommandHandler(IInventoryTransactionService service)
    : ICommandHandler<InventoryTransactionUpdateCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(InventoryTransactionUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
