using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public sealed class ImportTransactionCreateCommandHandler(IInventoryTransactionWorkflowService workflow)
    : ICommandHandler<ImportTransactionCreateCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(
        ImportTransactionCreateCommand command,
        CancellationToken cancellationToken)
    {
        var baseCommand = new InventoryTransactionCreateCommand
        {
            TransactionType = InventoryTransactionType.Receipt,
            TransactionDate = command.TransactionDate,
            DocumentNumber = command.DocumentNumber,
            TransactionPartyId = command.TransactionPartyId,
            BranchId = command.BranchId,
            Items = command.Items
        };

        return workflow.Create(baseCommand);
    }
}

public sealed class ImportTransactionUpdateCommandHandler(IInventoryTransactionWorkflowService workflow)
    : ICommandHandler<ImportTransactionUpdateCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(
        ImportTransactionUpdateCommand request,
        CancellationToken cancellationToken)
        => workflow.Update(request);
}

public sealed class ImportTransactionDeleteCommandHandler(IInventoryTransactionWorkflowService workflow)
    : ICommandHandler<ImportTransactionDeleteCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(
        ImportTransactionDeleteCommand request,
        CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}
