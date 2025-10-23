using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Inventory;

public class AttributeValueRepository : BaseSettingRepository<AttributeValue>, IAttributeValueRepository
{
    public AttributeValueRepository(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<AttributeValue>> GetByAttributeDefinitionIdAsync(Guid attributeDefinitionId)
    {
        return await _dbSet
            .Where(av => av.AttributeDefinitionId == attributeDefinitionId && av.IsActive)
            .OrderBy(av => av.SortOrder)
            .ThenBy(av => av.Name)
            .ToListAsync();
    }

    public async Task<List<AttributeValue>> GetActiveAttributeValuesAsync()
    {
        return await _dbSet
            .Where(av => av.IsActive)
            .Include(av => av.AttributeDefinition)
            .OrderBy(av => av.AttributeDefinition!.SortOrder)
            .ThenBy(av => av.SortOrder)
            .ThenBy(av => av.Name)
            .ToListAsync();
    }

    public async Task<AttributeValue?> GetByAttributeDefinitionAndNameAsync(Guid attributeDefinitionId, string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(av => av.AttributeDefinitionId == attributeDefinitionId && av.Name == name);
    }
}