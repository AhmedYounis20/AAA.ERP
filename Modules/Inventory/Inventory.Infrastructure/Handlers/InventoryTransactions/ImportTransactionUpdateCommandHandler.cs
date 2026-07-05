using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ImportTransactionUpdateCommandHandler(IImportTransactionService service)
    : ICommandHandler<ImportTransactionUpdateCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(ImportTransactionUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
