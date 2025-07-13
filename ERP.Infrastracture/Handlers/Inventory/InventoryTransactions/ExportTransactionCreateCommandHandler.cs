using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.InventoryTransactions;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace ERP.Infrastracture.Handlers.Inventory.InventoryTransactions;

public class ExportTransactionCreateCommandHandler(IExportTransactionService service) : ICommandHandler<ExportTransactionCreateCommand, ApiResponse<InventoryTransaction>>
{
    public async Task<ApiResponse<InventoryTransaction>> Handle(ExportTransactionCreateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
} 