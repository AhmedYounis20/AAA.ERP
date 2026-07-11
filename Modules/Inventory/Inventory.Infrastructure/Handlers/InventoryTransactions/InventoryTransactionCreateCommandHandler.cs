using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Handlers.InventoryTransactions;

public sealed class InventoryTransactionCreateCommandHandler(IInventoryTransactionWorkflowService workflow)
    : ICommandHandler<InventoryTransactionCreateCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(
        InventoryTransactionCreateCommand request,
        CancellationToken cancellationToken)
        => workflow.Create(request);
}

public sealed class InventoryTransactionUpdateCommandHandler(IInventoryTransactionWorkflowService workflow)
    : ICommandHandler<InventoryTransactionUpdateCommand, ApiResponse<InventoryTransaction>>
{
    public Task<ApiResponse<InventoryTransaction>> Handle(
        InventoryTransactionUpdateCommand request,
        CancellationToken cancellationToken)
        => workflow.Update(request);
}
