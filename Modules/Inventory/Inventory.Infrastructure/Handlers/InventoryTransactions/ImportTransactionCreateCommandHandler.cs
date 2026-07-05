using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ImportTransactionCreateCommandHandler(IImportTransactionService service)
    : ICommandHandler<ImportTransactionCreateCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(ImportTransactionCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
