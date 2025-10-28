using Inventory.Domain.Commands.InventoryTransactions;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ImportTransactionUpdateCommandHandler(IImportTransactionService service) : ICommandHandler<ImportTransactionUpdateCommand, ApiResponse<InventoryTransaction>>
{
    public async Task<ApiResponse<InventoryTransaction>> Handle(ImportTransactionUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
} 
