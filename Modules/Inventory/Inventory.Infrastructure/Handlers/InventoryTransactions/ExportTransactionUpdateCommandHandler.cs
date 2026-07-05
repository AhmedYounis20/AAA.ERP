using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ExportTransactionUpdateCommandHandler(IExportTransactionService service)
    : ICommandHandler<ExportTransactionUpdateCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(ExportTransactionUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
