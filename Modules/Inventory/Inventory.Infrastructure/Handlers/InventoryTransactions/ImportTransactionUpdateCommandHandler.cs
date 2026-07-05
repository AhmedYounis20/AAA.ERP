using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ImportTransactionUpdateCommandHandler(IImportTransactionService service)
    : ICommandHandler<ImportTransactionUpdateCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(ImportTransactionUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
