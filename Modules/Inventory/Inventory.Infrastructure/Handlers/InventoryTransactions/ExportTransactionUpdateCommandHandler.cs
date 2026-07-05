using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ExportTransactionUpdateCommandHandler(IExportTransactionService service)
    : ICommandHandler<ExportTransactionUpdateCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(ExportTransactionUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
