using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.Queries;
using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for attribute value GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IAttributeValueQueries : ISettingQueries<AttributeValue>
{
    Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetAttributeValuesByAttributeDefenitionId(Guid attributeId);

    Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetActiveAttributeValues();
}
