using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.Sizes;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Inventory.Infrastructure.Repositories;

public class SizeRepository : BaseSettingRepository<Size>, ISizeRepository
{
    public SizeRepository(IApplicationDbContext context) : base(context) { }

    public async Task<string?> GetMaxCodeAsync()
    {
        return await _dbSet
            .OrderByDescending(s => s.Code)
            .Select(s => s.Code)
            .FirstOrDefaultAsync();
    }
}
