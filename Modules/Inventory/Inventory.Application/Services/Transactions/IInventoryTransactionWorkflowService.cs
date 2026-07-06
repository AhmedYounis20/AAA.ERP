using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.Application.Services;

/// <summary>
/// Write-side workflow for inventory transaction orchestration (Category C).
/// </summary>
public interface IInventoryTransactionWorkflowService
{
    Task<ApiResponse<InventoryTransaction>> Create(InventoryTransactionCreateCommand command, bool isValidate = true);

    Task<ApiResponse<InventoryTransaction>> Update(InventoryTransactionUpdateCommand command);

    Task<ApiResponse<InventoryTransaction>> Delete(Guid id);
}
