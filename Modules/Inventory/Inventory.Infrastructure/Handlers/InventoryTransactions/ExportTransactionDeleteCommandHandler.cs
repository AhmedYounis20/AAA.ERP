using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ExportTransactionDeleteCommandHandler(IExportTransactionService exportTransactionService)
    : ICommandHandler<ExportTransactionDeleteCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(
        ExportTransactionDeleteCommand request,
        CancellationToken cancellationToken)
        => exportTransactionService.Delete(request.Id);
}
