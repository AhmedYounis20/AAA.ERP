using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Products;
using Inventory.Domain.Models.Dtos;

namespace Inventory.Application.Repositories
{
    public interface IProductRepository : IBaseTreeSettingRepository<Product>
    {
        Task<IEnumerable<ProductDto>> GetDtos();
        Task<ProductDto?> GetDtoById(Guid id);
        Task<IEnumerable<ProductDto>> GetProductsByType(ProductType? productType);
    }
}
