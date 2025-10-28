using Inventory.Domain.Models.Entities.Colors;

namespace Inventory.Infrastructure.Repositories;

public class ColorRepository : BaseSettingRepository<Color>, IColorRepository
{
    public ColorRepository(IInventoryDbContext context) : base(context) { }

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