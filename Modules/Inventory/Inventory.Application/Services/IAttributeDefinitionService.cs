using ERP.Application.Services.BaseServices;
using ERP.Domain.Models.Dtos.Inventory;
using Inventory.Application.Commands.AttributeDefinitions;
using ERP.Domain.Models.Entities.Inventory.Attributes;

namespace Inventory.Application.Services;

public interface IAttributeDefinitionService : IBaseSettingService<AttributeDefinition, AttributeDefinitionCreateCommand, AttributeDefinitionUpdateCommand>
{
    Task<AttributeDefinitionDto?> GetWithPredefinedValues(Guid id);
    Task<List<AttributeDefinitionDto>> GetActiveAttributeDefinitions();
}
