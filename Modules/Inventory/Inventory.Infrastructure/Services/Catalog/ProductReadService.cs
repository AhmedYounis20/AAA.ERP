using Inventory.Application.Dtos;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Products;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Product read queries. Transitional: delegates to <see cref="IProductService"/> until write methods are removed.
/// </summary>
public class ProductReadService(IProductService productService) : IProductReadService
{
    public Task<ApiResponse<List<ProductDto>>> GetAll()
        => productService.GetProductDtos();

    public Task<ApiResponse<ProductDto?>> GetById(Guid id)
        => productService.GetProductDtoById(id);

    public Task<ApiResponse<string>> GetNextCode(Guid? parentId = null)
        => productService.GetNextCode(parentId);

    public async Task<ApiResponse<IEnumerable<Product>>> GetLevel(int level = 0)
    {
        var items = await productService.GetLevel(level);
        return new ApiResponse<IEnumerable<Product>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }

    public async Task<ApiResponse<IEnumerable<Product>>> GetChildren(Guid parentId, int level = 0)
    {
        var items = await productService.GetChildren(parentId, level);
        return new ApiResponse<IEnumerable<Product>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }
}
