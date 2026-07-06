using Inventory.Application.Dtos;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.Infrastructure.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Attribute definition read queries.
/// </summary>
public class AttributeDefinitionQueries(
    IAttributeDefinitionRepository repository,
    IAttributeDefinitionService attributeDefinitionService)
    : BaseSettingRepositoryQueries<AttributeDefinition>(repository),
      IAttributeDefinitionQueries
{
    public Task<List<AttributeDefinitionDto>> GetActiveAttributeDefinitions()
        => attributeDefinitionService.GetActiveAttributeDefinitions();

    public Task<AttributeDefinitionDto?> GetWithPredefinedValues(Guid id)
        => attributeDefinitionService.GetWithPredefinedValues(id);
}
