using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side operations for attribute values (DTO projections and filtered lists).
/// </summary>
public interface IAttributeValueService
{
    Task<ApiResponse<List<AttributeValueDto>>> GetAttributeValueDtos();
    Task<ApiResponse<AttributeValueDto?>> GetAttributeValueDtoById(Guid id);
    Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetAttributeValuesByAttributeDefenitionId(Guid attributeId);
    Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetActiveAttributeValues();
}
