using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using ERP.Infrastracture.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastracture.Repositories.Inventory;

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