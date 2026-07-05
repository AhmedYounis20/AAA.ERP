using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class InventoryTransactionUpdateCommandHandler(IInventoryTransactionService service)
    : ICommandHandler<InventoryTransactionUpdateCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(InventoryTransactionUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
