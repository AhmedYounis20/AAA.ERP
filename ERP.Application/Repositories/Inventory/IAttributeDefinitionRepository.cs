using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;
using ERP.Domain.Models.Dtos.Inventory;

namespace ERP.Application.Repositories.Inventory;

public interface IAttributeDefinitionRepository : IBaseSettingRepository<AttributeDefinition>
{
    Task<AttributeDefinition?> GetWithPredefinedValuesAsync(Guid id);
    Task<List<AttributeDefinition>> GetActiveAttributeDefinitionsAsync();
}
