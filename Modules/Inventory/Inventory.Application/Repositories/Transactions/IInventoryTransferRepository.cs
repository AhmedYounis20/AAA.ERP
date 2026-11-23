using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities;

namespace Inventory.Application.Repositories;

/// <summary>
/// Repository contract for inventory transfer persistence.
/// </summary>
public interface IInventoryTransferRepository : IBaseRepository<InventoryTransfer>
{
    Task<IEnumerable<InventoryTransfer>> GetAllWithItems();

    /// <summary>Gets with details.</summary>
    Task<InventoryTransfer?> GetWithDetails(Guid id);
    Task<IEnumerable<InventoryTransfer>> GetByStatus(InventoryTransferStatus status);
}
