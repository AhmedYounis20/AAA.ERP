using Inventory.Application.Commands.Products;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Products;

namespace Inventory.Infrastructure.Workflows;

/// <summary>
/// Product write orchestration. Keeps write path decoupled from handler layer.
/// </summary>
public class ProductWorkflowService(IProductService productService) : IProductWorkflowService
{
    public Task<ApiResponse<Product>> Create(ProductCreateCommand command)
        => productService.Create(command);

    public Task<ApiResponse<Product>> Update(ProductUpdateCommand command)
        => productService.Update(command);

    public Task<ApiResponse<Product>> Delete(Guid id)
        => productService.Delete(id);
}
