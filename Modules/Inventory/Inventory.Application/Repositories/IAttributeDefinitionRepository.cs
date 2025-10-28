using Inventory.Domain.Models.Entities.Attributes;
using Inventory.Domain.Models.DTOS;
using Shared.Application.Repositories;

namespace Inventory.Application.Repositories;

public interface IAttributeDefinitionRepository : IBaseSettingRepository<AttributeDefinition>
{
    Task<AttributeDefinition?> GetWithPredefinedValuesAsync(Guid id);
    Task<List<AttributeDefinition>> GetActiveAttributeDefinitionsAsync();
}
