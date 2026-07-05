using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.InventoryTransfers;
using Inventory.Domain.Models.Entities;

using Inventory.Domain.Models.Dtos.Inventory;

namespace Inventory.Application.Services;

/// <summary>
/// Application service contract for inventory transfer.
/// </summary>
public interface IInventoryTransferService : IBaseService<InventoryTransfer, InventoryTransferCreateCommand, InventoryTransferUpdateCommand>
{
    Task<ApiResponse<InventoryTransferReadDto>> GetWithDetails(Guid id);
    Task<ApiResponse<IEnumerable<InventoryTransferReadDto>>> GetByStatus(InventoryTransferStatus status);
    Task<ApiResponse<InventoryTransfer>> ApproveTransfer(Guid id);
    Task<ApiResponse<InventoryTransfer>> RejectTransfer(Guid id, Guid approverId, string? reason);
}
