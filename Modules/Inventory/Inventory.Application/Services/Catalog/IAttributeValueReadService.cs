using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for attribute value GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IAttributeValueReadService
{
    Task<ApiResponse<IEnumerable<AttributeValue>>> GetAll();

    Task<ApiResponse<PaginatedResult<AttributeValue>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<AttributeValue>> GetById(Guid id);

    Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetAttributeValuesByAttributeDefenitionId(Guid attributeId);

    Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetActiveAttributeValues();
}
