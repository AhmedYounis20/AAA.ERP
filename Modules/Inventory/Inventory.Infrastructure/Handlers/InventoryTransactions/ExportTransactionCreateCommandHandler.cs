using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public class ExportTransactionCreateCommandHandler(IInventoryTransactionService inventoryTransactionService)
    : ICommandHandler<ExportTransactionCreateCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(
        ExportTransactionCreateCommand command,
        CancellationToken cancellationToken)
    {
        var baseCommand = new InventoryTransactionCreateCommand
        {
            TransactionType = InventoryTransactionType.Issue,
            TransactionDate = command.TransactionDate,
            DocumentNumber = command.DocumentNumber,
            TransactionPartyId = command.TransactionPartyId,
            BranchId = command.BranchId,
            Items = command.Items
        };

        return inventoryTransactionService.Create(baseCommand, isValidate: false);
    }
}
