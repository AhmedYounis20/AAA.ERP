using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Inventory.Products;
using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Domain.Models.Dtos.Inventory;

namespace ERP.Application.Services.Inventory;

public interface IProductService : IBaseTreeSettingService<Product, ProductCreateCommand, ProductUpdateCommand>
{
    Task<ApiResponse<string>> GetNextCode(Guid? parentId = null);
    Task<ApiResponse<List<ProductDto>>> GetProductDtos();
    Task<ApiResponse<ProductDto?>> GetProductDtoById(Guid id);
}
