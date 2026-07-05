using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ImportTransactionUpdateCommandHandler(IImportTransactionService importTransactionService)
    : ICommandHandler<ImportTransactionUpdateCommand, ApiResponse<Inventory.Domain.Models.Entities.InventoryTransactions.InventoryTransaction>>
{
    public Task<ApiResponse<Inventory.Domain.Models.Entities.InventoryTransactions.InventoryTransaction>> Handle(
        ImportTransactionUpdateCommand request,
        CancellationToken cancellationToken)
        => importTransactionService.Update(request);
}
