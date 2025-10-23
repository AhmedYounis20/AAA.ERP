using ERP.Application.Services.BaseServices;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;
using ERP.Domain.Models.Dtos.Inventory;
using ERP.Domain.Commands.Inventory.AttributeDefinitions;

namespace ERP.Application.Services.Inventory;

public interface IAttributeDefinitionService : IBaseSettingService<AttributeDefinition, AttributeDefinitionCreateCommand, AttributeDefinitionUpdateCommand>
{
    Task<AttributeDefinitionDto?> GetWithPredefinedValues(Guid id);
    Task<List<AttributeDefinitionDto>> GetActiveAttributeDefinitions();
}
