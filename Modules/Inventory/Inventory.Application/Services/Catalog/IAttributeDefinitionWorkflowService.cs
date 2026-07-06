using Inventory.Application.Commands.AttributeDefinitions;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.Application.Services;

/// <summary>
/// Write-side workflow for attribute definition orchestration (Category C).
/// </summary>
public interface IAttributeDefinitionWorkflowService
{
    Task<ApiResponse<AttributeDefinition>> Create(AttributeDefinitionCreateCommand command);

    Task<ApiResponse<AttributeDefinition>> Update(AttributeDefinitionUpdateCommand command);

    Task<ApiResponse<AttributeDefinition>> Delete(Guid id);
}
