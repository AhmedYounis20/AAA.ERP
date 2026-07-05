using Inventory.Application.Services;
using Inventory.Application.Commands.Products;
using Inventory.Domain.Models.Entities.Products;

namespace Inventory.Infrastructure.Handlers.Products;

public class ProductUpdateCommandHandler(IProductService service) : ICommandHandler<ProductUpdateCommand, ApiResponse<Product>>
{
    public async Task<ApiResponse<Product>> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}