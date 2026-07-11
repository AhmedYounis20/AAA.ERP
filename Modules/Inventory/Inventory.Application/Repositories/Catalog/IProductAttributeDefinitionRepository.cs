using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Products;

namespace Inventory.Application.Repositories;

/// <summary>
/// Repository contract for product attribute definition persistence.
/// </summary>
public interface IProductAttributeDefinitionRepository : IBaseRepository<ProductAttributeDefinition>
{
    Task<List<ProductAttributeDefinition>> GetByProductIdAsync(Guid productId);
    Task<List<ProductAttributeDefinition>> GetVariantAttributesByProductIdAsync(Guid productId);
}
