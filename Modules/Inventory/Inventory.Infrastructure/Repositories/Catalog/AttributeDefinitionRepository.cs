using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.Infrastructure.Repositories.BaseRepositories;

namespace Inventory.Infrastructure.Repositories;

public class AttributeDefinitionRepository : BaseSettingRepository<AttributeDefinition>, IAttributeDefinitionRepository
{
    public AttributeDefinitionRepository(IApplicationDbContext context) : base(context) { }

    public async Task<AttributeDefinition?> GetWithPredefinedValuesAsync(Guid id)
    {
        return await _dbSet
            .Include(ad => ad.PredefinedValues)
            .FirstOrDefaultAsync(ad => ad.Id == id);
    }

    public async Task<List<AttributeDefinition>> GetActiveAttributeDefinitionsAsync()
    {
        return await _dbSet
            .Where(ad => ad.IsActive)
            .Include(ad => ad.PredefinedValues.Where(av => av.IsActive))
            .OrderBy(ad => ad.SortOrder)
            .ToListAsync();
    }
}
