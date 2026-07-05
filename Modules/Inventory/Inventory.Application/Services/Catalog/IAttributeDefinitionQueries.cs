using ERP.BuildingBlocks.Application.Services.Queries;
using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for attribute definition GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IAttributeDefinitionQueries : ISettingQueries<AttributeDefinition>
{
    Task<List<AttributeDefinitionDto>> GetActiveAttributeDefinitions();

    Task<AttributeDefinitionDto?> GetWithPredefinedValues(Guid id);
}
