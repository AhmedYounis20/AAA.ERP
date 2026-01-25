using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Products;
using ERP.Domain.Models.Entities.Inventory.Products;

namespace ERP.Infrastracture.Handlers.Inventory.Products;

public class ProductCreateCommandHandler(IProductService service) : ICommandHandler<ProductCreateCommand, ApiResponse<Product>>
{
    public async Task<ApiResponse<Product>> Handle(ProductCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}