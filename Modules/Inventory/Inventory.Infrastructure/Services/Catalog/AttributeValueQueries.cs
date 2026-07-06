using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Attribute value read queries.
/// </summary>
public class AttributeValueQueries(
    IAttributeValueRepository repository,
    IAttributeValueService attributeValueService)
    : BaseSettingRepositoryQueries<AttributeValue>(repository),
      IAttributeValueQueries
{
    public Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetAttributeValuesByAttributeDefenitionId(Guid attributeId)
        => attributeValueService.GetAttributeValuesByAttributeDefenitionId(attributeId);

    public Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetActiveAttributeValues()
        => attributeValueService.GetActiveAttributeValues();
}
