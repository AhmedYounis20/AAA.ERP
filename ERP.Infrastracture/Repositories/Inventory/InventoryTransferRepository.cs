using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Inventory;

public class InventoryTransferRepository : BaseRepository<InventoryTransfer>, IInventoryTransferRepository
{
    public InventoryTransferRepository(IApplicationDbContext context) : base(context) { }

    public async Task<InventoryTransfer?> GetWithDetails(Guid id)
    {
        return await _dbSet
            .Include(e => e.SourceBranch)
            .Include(e => e.DestinationBranch)
            .Include(e => e.Items)
            .ThenInclude(i => i.Item)
            .Include(e => e.Items)
            .ThenInclude(i => i.PackingUnit)
            .FirstOrDefaultAsync(e => e.Id == id);
    }   
    public async Task<IEnumerable<InventoryTransfer>> GetByStatus(InventoryTransferStatus status)
    {
        return await _dbSet.Where(e => e.Status == status).ToListAsync();
    }
} 