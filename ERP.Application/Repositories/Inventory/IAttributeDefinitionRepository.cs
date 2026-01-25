using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Attributes;

namespace ERP.Application.Repositories.Inventory;

public interface IAttributeDefinitionRepository : IBaseSettingRepository<AttributeDefinition>
{
    Task<AttributeDefinition?> GetWithPredefinedValuesAsync(Guid id);
    Task<List<AttributeDefinition>> GetActiveAttributeDefinitionsAsync();
}
