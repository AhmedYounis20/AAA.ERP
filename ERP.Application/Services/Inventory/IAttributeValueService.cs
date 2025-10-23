using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Inventory.AttributeValues;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;
using ERP.Domain.Models.Dtos.Inventory;

namespace ERP.Application.Services.Inventory;

public interface IAttributeValueService : IBaseSettingService<AttributeValue, AttributeValueCreateCommand, AttributeValueUpdateCommand>
{
    Task<ApiResponse<List<AttributeValueDto>>> GetAttributeValueDtos();
    Task<ApiResponse<AttributeValueDto?>> GetAttributeValueDtoById(Guid id);
    Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetAttributeValuesByAttributeDefenitionId(Guid attributeId);
    Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetActiveAttributeValues();
    Task<ApiResponse<AttributeValueDto>> CreateAttributeValue(AttributeValueCreateCommand command);
    Task<ApiResponse<AttributeValueDto>> UpdateAttributeValue(AttributeValueUpdateCommand command);
}
