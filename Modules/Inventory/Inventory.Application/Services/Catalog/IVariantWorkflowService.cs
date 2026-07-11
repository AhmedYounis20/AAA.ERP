using Inventory.Application.Commands.Variants;
using Inventory.Domain.Models.Entities.Variants;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.Application.Services;

/// <summary>
/// Write-side workflow for variant CRUD orchestration (Category C).
/// </summary>
public interface IVariantWorkflowService
{
    Task<ApiResponse<Variant>> Create(VariantCreateCommand command);

    Task<ApiResponse<Variant>> Update(VariantUpdateCommand command);

    Task<ApiResponse<Variant>> Delete(Guid id);
}
