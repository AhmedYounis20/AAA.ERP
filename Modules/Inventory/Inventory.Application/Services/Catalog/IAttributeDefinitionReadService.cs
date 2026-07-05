using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for attribute definition GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IAttributeDefinitionReadService
{
    Task<ApiResponse<IEnumerable<AttributeDefinition>>> GetAll();

    Task<ApiResponse<PaginatedResult<AttributeDefinition>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<AttributeDefinition>> GetById(Guid id);

    Task<List<AttributeDefinitionDto>> GetActiveAttributeDefinitions();

    Task<AttributeDefinitionDto?> GetWithPredefinedValues(Guid id);
}
