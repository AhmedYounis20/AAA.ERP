using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Variants;

namespace ERP.Application.Repositories.Inventory;

public interface IVariantAttributeValueRepository : IBaseRepository<VariantAttributeValue>
{
    Task<List<VariantAttributeValue>> GetByVariantIdAsync(Guid variantId);
    Task<List<VariantAttributeValue>> GetByAttributeDefinitionIdAsync(Guid attributeDefinitionId);
    Task<List<VariantAttributeValue>> GetByAttributeValueIdAsync(Guid attributeValueId);
    Task<VariantAttributeValue?> GetByVariantAndAttributeDefinitionAsync(Guid variantId, Guid attributeDefinitionId);
    Task<List<VariantAttributeValue>> GetVariantsByAttributeValueAsync(Guid attributeDefinitionId, Guid attributeValueId);
}
