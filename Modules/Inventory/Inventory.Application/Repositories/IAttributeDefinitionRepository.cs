using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Application.Repositories;

public interface IAttributeDefinitionRepository : IBaseSettingRepository<AttributeDefinition>
{
    Task<AttributeDefinition?> GetWithPredefinedValuesAsync(Guid id);
    Task<List<AttributeDefinition>> GetActiveAttributeDefinitionsAsync();
}
