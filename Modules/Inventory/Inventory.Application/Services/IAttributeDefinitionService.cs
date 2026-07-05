using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Domain.Models.Dtos;
using Inventory.Application.Commands.AttributeDefinitions;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Application.Services;

/// <summary>
/// Application service contract for attribute definition.
/// </summary>
public interface IAttributeDefinitionService : IBaseSettingService<AttributeDefinition, AttributeDefinitionCreateCommand, AttributeDefinitionUpdateCommand>
{
    /// <summary>Gets with predefined values.</summary>
    Task<AttributeDefinitionDto?> GetWithPredefinedValues(Guid id);
    Task<List<AttributeDefinitionDto>> GetActiveAttributeDefinitions();
}
