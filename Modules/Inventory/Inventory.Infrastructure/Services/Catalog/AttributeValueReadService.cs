using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Attribute value read queries. Transitional: delegates to <see cref="IAttributeValueService"/> until write methods are removed.
/// </summary>
public class AttributeValueReadService(IAttributeValueService attributeValueService)
    : IAttributeValueReadService
{
    public Task<ApiResponse<IEnumerable<AttributeValue>>> GetAll()
        => attributeValueService.ReadAll();

    public Task<ApiResponse<PaginatedResult<AttributeValue>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default)
        => attributeValueService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<AttributeValue>> GetById(Guid id)
        => attributeValueService.ReadById(id);

    public Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetAttributeValuesByAttributeDefenitionId(Guid attributeId)
        => attributeValueService.GetAttributeValuesByAttributeDefenitionId(attributeId);

    public Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetActiveAttributeValues()
        => attributeValueService.GetActiveAttributeValues();
}
