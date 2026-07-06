using Inventory.Application.Commands.Variants;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Variants;

namespace Inventory.Infrastructure.Workflows;

/// <summary>
/// Variant write orchestration. Keeps write path decoupled from handler layer.
/// </summary>
public class VariantWorkflowService(IVariantService variantService) : IVariantWorkflowService
{
    public Task<ApiResponse<Variant>> Create(VariantCreateCommand command)
        => variantService.Create(command);

    public Task<ApiResponse<Variant>> Update(VariantUpdateCommand command)
        => variantService.Update(command);

    public Task<ApiResponse<Variant>> Delete(Guid id)
        => variantService.Delete(id);
}
