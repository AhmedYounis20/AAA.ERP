using Inventory.Application.Commands.Items;
using Inventory.Domain.Models.Entities.Items;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.Application.Services;

/// <summary>
/// Write-side workflow for item CRUD orchestration (Category C).
/// </summary>
public interface IItemWorkflowService
{
    Task<ApiResponse<Item>> Create(ItemCreateCommand command);

    Task<ApiResponse<Item>> Update(ItemUpdateCommand command);

    Task<ApiResponse<Item>> Delete(Guid id);
}
