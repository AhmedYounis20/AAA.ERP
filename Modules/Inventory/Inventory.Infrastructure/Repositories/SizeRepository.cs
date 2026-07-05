using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.Sizes;
using ERP.Infrastructure.Repositories.BaseRepositories;

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
