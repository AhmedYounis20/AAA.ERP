using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Products;
using Inventory.Application.Dtos;

namespace Inventory.Application.Repositories
{
    /// <summary>
    /// Repository contract for product persistence.
    /// </summary>
    public interface IProductRepository : IBaseTreeSettingRepository<Product>
    {
        Task<IEnumerable<ProductDto>> GetDtos();
        /// <summary>Gets dto by id.</summary>
        Task<ProductDto?> GetDtoById(Guid id);
        Task<IEnumerable<ProductDto>> GetProductsByType(ProductType? productType);
    }
}
