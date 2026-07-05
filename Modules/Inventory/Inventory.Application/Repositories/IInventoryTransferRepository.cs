using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities;

namespace Inventory.Application.Repositories;

public interface IInventoryTransferRepository : IBaseRepository<InventoryTransfer>
{
    Task<InventoryTransfer?> GetWithDetails(Guid id);
    Task<IEnumerable<InventoryTransfer>> GetByStatus(InventoryTransferStatus status);
} 