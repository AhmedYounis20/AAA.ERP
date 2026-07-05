using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Attribute definition read queries. Transitional: delegates to <see cref="IAttributeDefinitionService"/> until write methods are removed.
/// </summary>
public class AttributeDefinitionReadService(IAttributeDefinitionService attributeDefinitionService)
    : IAttributeDefinitionReadService
{
    public Task<ApiResponse<IEnumerable<AttributeDefinition>>> GetAll()
        => attributeDefinitionService.ReadAll();

    public Task<ApiResponse<PaginatedResult<AttributeDefinition>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default)
        => attributeDefinitionService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<AttributeDefinition>> GetById(Guid id)
        => attributeDefinitionService.ReadById(id);

    public Task<List<AttributeDefinitionDto>> GetActiveAttributeDefinitions()
        => attributeDefinitionService.GetActiveAttributeDefinitions();

    public Task<AttributeDefinitionDto?> GetWithPredefinedValues(Guid id)
        => attributeDefinitionService.GetWithPredefinedValues(id);
}
