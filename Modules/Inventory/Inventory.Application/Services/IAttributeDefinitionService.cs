using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Domain.Models.Dtos;
using Inventory.Application.Commands.AttributeDefinitions;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Application.Services;

public interface IAttributeDefinitionService : IBaseSettingService<AttributeDefinition, AttributeDefinitionCreateCommand, AttributeDefinitionUpdateCommand>
{
    Task<AttributeDefinitionDto?> GetWithPredefinedValues(Guid id);
    Task<List<AttributeDefinitionDto>> GetActiveAttributeDefinitions();
}
