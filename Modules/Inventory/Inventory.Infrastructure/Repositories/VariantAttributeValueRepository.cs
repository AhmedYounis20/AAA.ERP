using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace Inventory.Infrastructure.Repositories;

public class VariantAttributeValueRepository : BaseRepository<VariantAttributeValue>, IVariantAttributeValueRepository
{
    public VariantAttributeValueRepository(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<VariantAttributeValue>> GetByVariantIdAsync(Guid variantId)
    {
        return await _dbSet
            .Where(vav => vav.VariantId == variantId)
            .Include(vav => vav.AttributeDefinition)
            .Include(vav => vav.AttributeValue)
            .OrderBy(vav => vav.AttributeDefinition!.SortOrder)
            .ToListAsync();
    }

    public async Task<List<VariantAttributeValue>> GetByAttributeDefinitionIdAsync(Guid attributeDefinitionId)
    {
        return await _dbSet
            .Where(vav => vav.AttributeDefinitionId == attributeDefinitionId)
            .Include(vav => vav.Variant)
            .Include(vav => vav.AttributeValue)
            .ToListAsync();
    }

    public async Task<List<VariantAttributeValue>> GetByAttributeValueIdAsync(Guid attributeValueId)
    {
        return await _dbSet
            .Where(vav => vav.AttributeValueId == attributeValueId)
            .Include(vav => vav.Variant)
            .Include(vav => vav.AttributeDefinition)
            .ToListAsync();
    }

    public async Task<VariantAttributeValue?> GetByVariantAndAttributeDefinitionAsync(Guid variantId, Guid attributeDefinitionId)
    {
        return await _dbSet
            .Include(vav => vav.AttributeValue)
            .FirstOrDefaultAsync(vav => vav.VariantId == variantId && vav.AttributeDefinitionId == attributeDefinitionId);
    }

    public async Task<List<VariantAttributeValue>> GetVariantsByAttributeValueAsync(Guid attributeDefinitionId, Guid attributeValueId)
    {
        return await _dbSet
            .Where(vav => vav.AttributeDefinitionId == attributeDefinitionId && vav.AttributeValueId == attributeValueId)
            .Include(vav => vav.Variant)
            .ThenInclude(v => v!.Product)
            .ToListAsync();
    }
}





