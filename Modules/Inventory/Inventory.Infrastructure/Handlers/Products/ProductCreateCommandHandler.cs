using Inventory.Application.Services;
using Inventory.Application.Commands.Products;
using Inventory.Domain.Models.Entities.Products;

namespace Inventory.Infrastructure.Handlers.Products;

public class ProductCreateCommandHandler(IProductService service) : ICommandHandler<ProductCreateCommand, ApiResponse<Product>>
{
    public async Task<ApiResponse<Product>> Handle(ProductCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}