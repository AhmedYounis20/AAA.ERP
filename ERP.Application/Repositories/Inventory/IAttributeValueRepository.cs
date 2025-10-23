using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Application.Repositories.Inventory;

public interface IAttributeValueRepository : IBaseSettingRepository<AttributeValue>
{
    Task<List<AttributeValue>> GetByAttributeDefinitionIdAsync(Guid attributeDefinitionId);
    Task<List<AttributeValue>> GetActiveAttributeValuesAsync();
    Task<AttributeValue?> GetByAttributeDefinitionAndNameAsync(Guid attributeDefinitionId, string name);
}