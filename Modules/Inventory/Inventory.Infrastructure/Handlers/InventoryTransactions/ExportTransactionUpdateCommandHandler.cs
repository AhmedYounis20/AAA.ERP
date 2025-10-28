using Inventory.Domain.Commands.InventoryTransactions;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ExportTransactionUpdateCommandHandler(IExportTransactionService service) : ICommandHandler<ExportTransactionUpdateCommand, ApiResponse<InventoryTransaction>>
{
    public async Task<ApiResponse<InventoryTransaction>> Handle(ExportTransactionUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
} 
