using Inventory.Application.Commands.InventoryTransfers;
using Inventory.Domain.Models.Entities;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.Application.Services;

/// <summary>
/// Write-side workflow for inventory transfer orchestration (Category C).
/// </summary>
public interface IInventoryTransferWorkflowService
{
    Task<ApiResponse<InventoryTransfer>> Create(InventoryTransferCreateCommand command);

    Task<ApiResponse<InventoryTransfer>> Update(InventoryTransferUpdateCommand command);

    Task<ApiResponse<InventoryTransfer>> ApproveTransfer(Guid id);

    Task<ApiResponse<InventoryTransfer>> RejectTransfer(Guid id, Guid approverId, string? reason);
}
