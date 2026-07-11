using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Application.Repositories;

/// <summary>
/// Repository contract for attribute value persistence.
/// </summary>
public interface IAttributeValueRepository : IBaseSettingRepository<AttributeValue>
{
    Task<List<AttributeValue>> GetByAttributeDefinitionIdAsync(Guid attributeDefinitionId);
    Task<List<AttributeValue>> GetActiveAttributeValuesAsync();
    /// <summary>Gets by attribute definition and name async.</summary>
    Task<AttributeValue?> GetByAttributeDefinitionAndNameAsync(Guid attributeDefinitionId, string name);
}
