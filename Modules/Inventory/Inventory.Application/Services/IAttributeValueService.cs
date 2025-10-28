using Inventory.Domain.Models.Entities.Attributes;
using Inventory.Domain.Commands.AttributeValues;
using Inventory.Domain.Models.DTOS;
using Shared.Application.BaseServices;
using Shared.Domain.Responses;

namespace Inventory.Application.Services;

public interface IAttributeValueService : IBaseSettingService<AttributeValue, AttributeValueCreateCommand, AttributeValueUpdateCommand>
{
    Task<ApiResponse<List<AttributeValueDto>>> GetAttributeValueDtos();
    Task<ApiResponse<AttributeValueDto?>> GetAttributeValueDtoById(Guid id);
    Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetAttributeValuesByAttributeDefenitionId(Guid attributeId);
    Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetActiveAttributeValues();
    Task<ApiResponse<AttributeValueDto>> CreateAttributeValue(AttributeValueCreateCommand command);
    Task<ApiResponse<AttributeValueDto>> UpdateAttributeValue(AttributeValueUpdateCommand command);
}
