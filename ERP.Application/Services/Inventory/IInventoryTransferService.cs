using ERP.Application.Services.BaseServices;
using ERP.Domain.Models.Entities.Inventory;
using ERP.Domain.Commands.Inventory;

namespace ERP.Application.Services.Inventory;

public interface IInventoryTransferService : IBaseService<InventoryTransfer, InventoryTransferCreateCommand, InventoryTransferUpdateCommand>
{
    Task<ApiResponse<InventoryTransfer>> GetWithDetails(Guid id);
    Task<ApiResponse<IEnumerable<InventoryTransfer>>> GetByStatus(InventoryTransferStatus status);
    Task<ApiResponse<InventoryTransfer>> ApproveTransfer(Guid id);
    Task<ApiResponse<InventoryTransfer>> RejectTransfer(Guid id, Guid approverId, string? reason);
} 