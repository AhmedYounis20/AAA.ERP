using Inventory.Application.Dtos;

namespace Inventory.Application.Services;

/// <summary>
/// Attribute definition read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public interface IAttributeDefinitionService
{
    Task<AttributeDefinitionDto?> GetWithPredefinedValues(Guid id);

    Task<List<AttributeDefinitionDto>> GetActiveAttributeDefinitions();
}
