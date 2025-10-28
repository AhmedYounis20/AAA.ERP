using ERP.Application.Data;
using ERP.Application.Repositories.Inventory;
using Inventory.Domain.Models.Entities.Sizes;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class SizeRepository : BaseSettingRepository<Size>, ISizeRepository
{
    public SizeRepository(IInventoryDbContext context) : base(context) { }

    public async Task<string?> GetMaxCodeAsync()
    {
        return await _dbSet
            .OrderByDescending(s => s.Code)
            .Select(s => s.Code)
            .FirstOrDefaultAsync();
    }
}
