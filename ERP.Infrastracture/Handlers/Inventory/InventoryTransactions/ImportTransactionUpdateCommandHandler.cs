using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.InventoryTransactions;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace ERP.Infrastracture.Handlers.Inventory.InventoryTransactions;

public class ImportTransactionUpdateCommandHandler(IImportTransactionService service) : ICommandHandler<ImportTransactionUpdateCommand, ApiResponse<InventoryTransaction>>
{
    public async Task<ApiResponse<InventoryTransaction>> Handle(ImportTransactionUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
} 