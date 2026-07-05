using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Domain.Models.Dtos.Inventory;

namespace ERP.Application.Repositories.Inventory
{
    public interface IProductRepository : IBaseTreeSettingRepository<Product>
    {
        Task<IEnumerable<ProductDto>> GetDtos();
        Task<ProductDto?> GetDtoById(Guid id);
        Task<IEnumerable<ProductDto>> GetProductsByType(ProductType? productType);
    }
}
