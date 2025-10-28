using Inventory.Domain.Commands.InventoryTransactions;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ImportTransactionCreateCommandHandler(IImportTransactionService service) : ICommandHandler<ImportTransactionCreateCommand, ApiResponse<InventoryTransaction>>
{
    public async Task<ApiResponse<InventoryTransaction>> Handle(ImportTransactionCreateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
} 