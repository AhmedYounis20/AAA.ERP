using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Variants;

namespace Inventory.Application.Repositories;

public interface IVariantAttributeValueRepository : IBaseRepository<VariantAttributeValue>
{
    Task<List<VariantAttributeValue>> GetByVariantIdAsync(Guid variantId);
    Task<List<VariantAttributeValue>> GetByAttributeDefinitionIdAsync(Guid attributeDefinitionId);
    Task<List<VariantAttributeValue>> GetByAttributeValueIdAsync(Guid attributeValueId);
    Task<VariantAttributeValue?> GetByVariantAndAttributeDefinitionAsync(Guid variantId, Guid attributeDefinitionId);
    Task<List<VariantAttributeValue>> GetVariantsByAttributeValueAsync(Guid attributeDefinitionId, Guid attributeValueId);
}
