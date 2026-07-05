using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ExportTransactionUpdateCommandHandler(IExportTransactionService exportTransactionService)
    : ICommandHandler<ExportTransactionUpdateCommand, ApiResponse<Inventory.Domain.Models.Entities.InventoryTransactions.InventoryTransaction>>
{
    public Task<ApiResponse<Inventory.Domain.Models.Entities.InventoryTransactions.InventoryTransaction>> Handle(
        ExportTransactionUpdateCommand request,
        CancellationToken cancellationToken)
        => exportTransactionService.Update(request);
}
