using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Products;
using ERP.Domain.Models.Entities.Inventory.Products;

namespace ERP.Infrastracture.Handlers.Inventory.Products;

public class ProductUpdateCommandHandler(IProductService service) : ICommandHandler<ProductUpdateCommand, ApiResponse<Product>>
{
    public async Task<ApiResponse<Product>> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}