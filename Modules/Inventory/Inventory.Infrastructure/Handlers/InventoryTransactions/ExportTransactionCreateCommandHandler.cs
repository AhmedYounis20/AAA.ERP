using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ExportTransactionCreateCommandHandler(IExportTransactionService service)
    : ICommandHandler<ExportTransactionCreateCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(ExportTransactionCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
