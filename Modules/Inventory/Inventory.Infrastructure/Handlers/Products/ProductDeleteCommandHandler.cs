using Inventory.Application.Commands.Products;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Products;

namespace Inventory.Infrastructure.Handlers.Products;

public class ProductDeleteCommandHandler(IProductService productService)
    : ICommandHandler<ProductDeleteCommand, ApiResponse<Product>>
{
    public Task<ApiResponse<Product>> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        => productService.Delete(request.Id);
}
