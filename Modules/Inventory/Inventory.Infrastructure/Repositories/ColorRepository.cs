using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.Colors;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace Inventory.Infrastructure.Repositories;

public class ColorRepository : BaseSettingRepository<Color>, IColorRepository
{
    public ColorRepository(IApplicationDbContext context) : base(context) { }

    public async Task<string?> GetMaxCodeAsync()
    {
        return await _dbSet
            .OrderByDescending(c => c.Code)
            .Select(c => c.Code)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> GetByColorValueExists(string colorValue)
    {
        return await _dbSet
            .AnyAsync(c => c.ColorValue.Trim().ToUpper() == colorValue.Trim().ToUpper());
    }
}
