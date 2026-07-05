using Inventory.Application.Commands.AttributeDefinitions;
using Inventory.Application.Dtos;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.Infrastructure.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Attribute definition read queries. Transitional: delegates to <see cref="IAttributeDefinitionService"/> until write methods are removed.
/// </summary>
public class AttributeDefinitionQueries(IAttributeDefinitionService attributeDefinitionService)
    : BaseSettingQueries<AttributeDefinition, AttributeDefinitionCreateCommand, AttributeDefinitionUpdateCommand>(attributeDefinitionService),
      IAttributeDefinitionQueries
{
    public Task<List<AttributeDefinitionDto>> GetActiveAttributeDefinitions()
        => attributeDefinitionService.GetActiveAttributeDefinitions();

    public Task<AttributeDefinitionDto?> GetWithPredefinedValues(Guid id)
        => attributeDefinitionService.GetWithPredefinedValues(id);
}
