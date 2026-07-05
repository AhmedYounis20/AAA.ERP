using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Application.Repositories;

public interface IAttributeValueRepository : IBaseSettingRepository<AttributeValue>
{
    Task<List<AttributeValue>> GetByAttributeDefinitionIdAsync(Guid attributeDefinitionId);
    Task<List<AttributeValue>> GetActiveAttributeValuesAsync();
    Task<AttributeValue?> GetByAttributeDefinitionAndNameAsync(Guid attributeDefinitionId, string name);
}