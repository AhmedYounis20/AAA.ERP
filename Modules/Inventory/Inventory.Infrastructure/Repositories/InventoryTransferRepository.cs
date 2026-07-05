using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities;
using ERP.Infrastructure.Repositories.BaseRepositories;

namespace Inventory.Infrastructure.Repositories;

public class InventoryTransferRepository : BaseRepository<InventoryTransfer>, IInventoryTransferRepository
{
    public InventoryTransferRepository(IApplicationDbContext context) : base(context) { }

    public async Task<InventoryTransfer?> GetWithDetails(Guid id)
    {
        return await _dbSet
            .Include(e => e.Items)
            .ThenInclude(i => i.Variant)
            .Include(e => e.Items)
            .ThenInclude(i => i.PackingUnit)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<InventoryTransfer>> GetByStatus(InventoryTransferStatus status)
    {
        return await _dbSet.Where(e => e.Status == status).ToListAsync();
    }
}
