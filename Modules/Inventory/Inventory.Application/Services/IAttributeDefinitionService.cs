using Inventory.Domain.Models.Entities.Attributes;
using Inventory.Domain.Commands.AttributeDefinitions;
using Inventory.Domain.Models.DTOS;
using Shared.Application.BaseServices;

namespace Inventory.Application.Services;

public interface IAttributeDefinitionService : IBaseSettingService<AttributeDefinition, AttributeDefinitionCreateCommand, AttributeDefinitionUpdateCommand>
{
    Task<AttributeDefinitionDto?> GetWithPredefinedValues(Guid id);
    Task<List<AttributeDefinitionDto>> GetActiveAttributeDefinitions();
}
