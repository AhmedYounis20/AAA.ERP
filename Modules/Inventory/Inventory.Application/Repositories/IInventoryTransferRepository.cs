using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory;

namespace ERP.Application.Repositories.Inventory;

public interface IInventoryTransferRepository : IBaseRepository<InventoryTransfer>
{
    Task<InventoryTransfer?> GetWithDetails(Guid id);
    Task<IEnumerable<InventoryTransfer>> GetByStatus(InventoryTransferStatus status);
} 