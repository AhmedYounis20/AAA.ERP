using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class InventoryTransactionCreateCommandHandler(IInventoryTransactionService service)
    : ICommandHandler<InventoryTransactionCreateCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(InventoryTransactionCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
