using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace Inventory.Infrastructure.Repositories;

public class ProductAttributeDefinitionRepository : BaseRepository<ProductAttributeDefinition>, IProductAttributeDefinitionRepository
{
    public ProductAttributeDefinitionRepository(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<ProductAttributeDefinition>> GetByProductIdAsync(Guid productId)
    {
        return await _dbSet
            .Where(pad => pad.ProductId == productId)
            .Include(pad => pad.AttributeDefinition)
            .ThenInclude(ad => ad!.PredefinedValues.Where(av => av.IsActive))
            .OrderBy(pad => pad.AttributeDefinition!.SortOrder)
            .ToListAsync();
    }

    public async Task<List<ProductAttributeDefinition>> GetVariantAttributesByProductIdAsync(Guid productId)
    {
        return await _dbSet
            .Where(pad => pad.ProductId == productId && pad.IsVariant)
            .Include(pad => pad.AttributeDefinition)
            .ThenInclude(ad => ad!.PredefinedValues.Where(av => av.IsActive))
            .OrderBy(pad => pad.AttributeDefinition!.SortOrder)
            .ToListAsync();
    }

    public async Task<ProductAttributeDefinition?> GetByProductAndAttributeDefinitionAsync(Guid productId, Guid attributeDefinitionId)
    {
        return await _dbSet
            .Include(pad => pad.AttributeDefinition)
            .FirstOrDefaultAsync(pad => pad.ProductId == productId && pad.AttributeDefinitionId == attributeDefinitionId);
    }
}





