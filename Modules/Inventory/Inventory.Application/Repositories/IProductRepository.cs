using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Domain.Models.Dtos.Inventory;

namespace Inventory.Application.Repositories
{
    public interface IProductRepository : IBaseTreeSettingRepository<Product>
    {
        Task<IEnumerable<ProductDto>> GetDtos();
        Task<ProductDto?> GetDtoById(Guid id);
        Task<IEnumerable<ProductDto>> GetProductsByType(ProductType? productType);
    }
}
