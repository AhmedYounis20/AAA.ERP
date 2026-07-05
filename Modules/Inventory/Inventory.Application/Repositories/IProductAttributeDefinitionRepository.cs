using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Products;

namespace ERP.Application.Repositories.Inventory;

public interface IProductAttributeDefinitionRepository : IBaseRepository<ProductAttributeDefinition>
{
    Task<List<ProductAttributeDefinition>> GetByProductIdAsync(Guid productId);
    Task<List<ProductAttributeDefinition>> GetVariantAttributesByProductIdAsync(Guid productId);
}
