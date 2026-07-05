using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Variants;

namespace Inventory.Application.Repositories;

/// <summary>
/// Repository contract for variant attribute value persistence.
/// </summary>
public interface IVariantAttributeValueRepository : IBaseRepository<VariantAttributeValue>
{
    Task<List<VariantAttributeValue>> GetByVariantIdAsync(Guid variantId);
    Task<List<VariantAttributeValue>> GetByAttributeDefinitionIdAsync(Guid attributeDefinitionId);
    Task<List<VariantAttributeValue>> GetByAttributeValueIdAsync(Guid attributeValueId);
    /// <summary>Gets by variant and attribute definition async.</summary>
    Task<VariantAttributeValue?> GetByVariantAndAttributeDefinitionAsync(Guid variantId, Guid attributeDefinitionId);
    Task<List<VariantAttributeValue>> GetVariantsByAttributeValueAsync(Guid attributeDefinitionId, Guid attributeValueId);
}
