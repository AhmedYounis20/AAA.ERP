using Inventory.Application.Commands.Products;
using Inventory.Domain.Models.Entities.Products;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.Application.Services;

/// <summary>
/// Write-side workflow for product CRUD orchestration (Category C).
/// </summary>
public interface IProductWorkflowService
{
    Task<ApiResponse<Product>> Create(ProductCreateCommand command);

    Task<ApiResponse<Product>> Update(ProductUpdateCommand command);

    Task<ApiResponse<Product>> Delete(Guid id);
}
