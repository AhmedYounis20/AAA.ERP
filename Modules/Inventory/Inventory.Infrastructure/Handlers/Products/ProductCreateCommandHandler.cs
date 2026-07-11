using Inventory.Application.Services;
using Inventory.Application.Commands.Products;
using Inventory.Domain.Models.Entities.Products;

namespace Inventory.Infrastructure.Handlers.Products;

public sealed class ProductCreateCommandHandler(IProductWorkflowService workflow)
    : ICommandHandler<ProductCreateCommand, ApiResponse<Product>>
{
    public Task<ApiResponse<Product>> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        => workflow.Create(request);
}

public sealed class ProductUpdateCommandHandler(IProductWorkflowService workflow)
    : ICommandHandler<ProductUpdateCommand, ApiResponse<Product>>
{
    public Task<ApiResponse<Product>> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        => workflow.Update(request);
}

public sealed class ProductDeleteCommandHandler(IProductWorkflowService workflow)
    : ICommandHandler<ProductDeleteCommand, ApiResponse<Product>>
{
    public Task<ApiResponse<Product>> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}