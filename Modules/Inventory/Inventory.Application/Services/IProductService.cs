using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.Products;
using Inventory.Domain.Models.Entities.Products;
using Inventory.Domain.Models.Dtos;

namespace Inventory.Application.Services;

public interface IProductService : IBaseTreeSettingService<Product, ProductCreateCommand, ProductUpdateCommand>
{
    Task<ApiResponse<string>> GetNextCode(Guid? parentId = null);
    Task<ApiResponse<List<ProductDto>>> GetProductDtos();
    Task<ApiResponse<ProductDto?>> GetProductDtoById(Guid id);
}
