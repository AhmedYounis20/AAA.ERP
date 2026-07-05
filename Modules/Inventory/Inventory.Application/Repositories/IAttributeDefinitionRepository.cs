using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Application.Repositories;

/// <summary>
/// Repository contract for attribute definition persistence.
/// </summary>
public interface IAttributeDefinitionRepository : IBaseSettingRepository<AttributeDefinition>
{
    /// <summary>Gets with predefined values async.</summary>
    Task<AttributeDefinition?> GetWithPredefinedValuesAsync(Guid id);
    Task<List<AttributeDefinition>> GetActiveAttributeDefinitionsAsync();
}
