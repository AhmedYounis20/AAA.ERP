using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ImportTransactionDeleteCommandHandler(IImportTransactionService importTransactionService)
    : ICommandHandler<ImportTransactionDeleteCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(
        ImportTransactionDeleteCommand request,
        CancellationToken cancellationToken)
        => importTransactionService.Delete(request.Id);
}
