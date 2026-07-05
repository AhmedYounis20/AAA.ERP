using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.InventoryTransfers;
using Inventory.Domain.Models.Entities;

namespace Inventory.Application.Services;

public interface IInventoryTransferService : IBaseService<InventoryTransfer, InventoryTransferCreateCommand, InventoryTransferUpdateCommand>
{
    Task<ApiResponse<InventoryTransfer>> GetWithDetails(Guid id);
    Task<ApiResponse<IEnumerable<InventoryTransfer>>> GetByStatus(InventoryTransferStatus status);
    Task<ApiResponse<InventoryTransfer>> ApproveTransfer(Guid id);
    Task<ApiResponse<InventoryTransfer>> RejectTransfer(Guid id, Guid approverId, string? reason);
}
