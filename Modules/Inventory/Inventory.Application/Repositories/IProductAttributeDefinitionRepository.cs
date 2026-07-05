using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Products;

namespace Inventory.Application.Repositories;

public interface IProductAttributeDefinitionRepository : IBaseRepository<ProductAttributeDefinition>
{
    Task<List<ProductAttributeDefinition>> GetByProductIdAsync(Guid productId);
    Task<List<ProductAttributeDefinition>> GetVariantAttributesByProductIdAsync(Guid productId);
}
