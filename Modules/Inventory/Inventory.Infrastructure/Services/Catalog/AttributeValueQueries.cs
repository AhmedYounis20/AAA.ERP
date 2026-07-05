using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Commands.AttributeValues;
using Inventory.Application.Dtos;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.Infrastructure.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Attribute value read queries. Transitional: delegates to <see cref="IAttributeValueService"/> until write methods are removed.
/// </summary>
public class AttributeValueQueries(IAttributeValueService attributeValueService)
    : BaseSettingQueries<AttributeValue, AttributeValueCreateCommand, AttributeValueUpdateCommand>(attributeValueService),
      IAttributeValueQueries
{
    public Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetAttributeValuesByAttributeDefenitionId(Guid attributeId)
        => attributeValueService.GetAttributeValuesByAttributeDefenitionId(attributeId);

    public Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetActiveAttributeValues()
        => attributeValueService.GetActiveAttributeValues();
}
