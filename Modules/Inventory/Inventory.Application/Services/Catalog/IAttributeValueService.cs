using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.AttributeValues;
using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Application.Services;

/// <summary>
/// Application service contract for attribute value.
/// </summary>
public interface IAttributeValueService : IBaseSettingService<AttributeValue, AttributeValueCreateCommand, AttributeValueUpdateCommand>
{
    Task<ApiResponse<List<AttributeValueDto>>> GetAttributeValueDtos();
    Task<ApiResponse<AttributeValueDto?>> GetAttributeValueDtoById(Guid id);
    Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetAttributeValuesByAttributeDefenitionId(Guid attributeId);
    Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetActiveAttributeValues();
    Task<ApiResponse<AttributeValueDto>> CreateAttributeValue(AttributeValueCreateCommand command);
    Task<ApiResponse<AttributeValueDto>> UpdateAttributeValue(AttributeValueUpdateCommand command);
}
