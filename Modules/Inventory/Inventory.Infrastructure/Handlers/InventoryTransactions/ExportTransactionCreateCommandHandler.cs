using Inventory.Domain.Commands.InventoryTransactions;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ExportTransactionCreateCommandHandler(IExportTransactionService service) : ICommandHandler<ExportTransactionCreateCommand, ApiResponse<InventoryTransaction>>
{
    public async Task<ApiResponse<InventoryTransaction>> Handle(ExportTransactionCreateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
} 
