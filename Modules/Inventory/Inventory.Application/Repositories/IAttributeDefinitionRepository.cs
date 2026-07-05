using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Attributes;

namespace Inventory.Application.Repositories;

public interface IAttributeDefinitionRepository : IBaseSettingRepository<AttributeDefinition>
{
    Task<AttributeDefinition?> GetWithPredefinedValuesAsync(Guid id);
    Task<List<AttributeDefinition>> GetActiveAttributeDefinitionsAsync();
}
