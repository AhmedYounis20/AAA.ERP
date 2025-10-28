using Inventory.Domain.Models.Entities;
using Shared.Application.Repositories;

namespace ERP.Application.Repositories.Inventory;

public interface IInventoryTransferRepository : IBaseRepository<InventoryTransfer>
{
    Task<InventoryTransfer?> GetWithDetails(Guid id);
    Task<IEnumerable<InventoryTransfer>> GetByStatus(InventoryTransferStatus status);
} 