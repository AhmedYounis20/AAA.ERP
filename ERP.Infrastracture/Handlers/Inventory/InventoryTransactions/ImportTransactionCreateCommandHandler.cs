using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.InventoryTransactions;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace ERP.Infrastracture.Handlers.Inventory.InventoryTransactions;

public class ImportTransactionCreateCommandHandler(IImportTransactionService service) : ICommandHandler<ImportTransactionCreateCommand, ApiResponse<InventoryTransaction>>
{
    public async Task<ApiResponse<InventoryTransaction>> Handle(ImportTransactionCreateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
} 