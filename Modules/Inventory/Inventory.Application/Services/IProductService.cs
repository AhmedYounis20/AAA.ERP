using ERP.Application.Services.BaseServices;
using Inventory.Application.Commands.Products;
using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Domain.Models.Dtos.Inventory;

namespace Inventory.Application.Services;

public interface IProductService : IBaseTreeSettingService<Product, ProductCreateCommand, ProductUpdateCommand>
{
    Task<ApiResponse<string>> GetNextCode(Guid? parentId = null);
    Task<ApiResponse<List<ProductDto>>> GetProductDtos();
    Task<ApiResponse<ProductDto?>> GetProductDtoById(Guid id);
}
