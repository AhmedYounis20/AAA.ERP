using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.Products;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for product GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IProductQueries
{
    Task<ApiResponse<List<ProductDto>>> GetAll();

    Task<ApiResponse<ProductDto?>> GetById(Guid id);

    Task<ApiResponse<string>> GetNextCode(Guid? parentId = null);

    Task<ApiResponse<IEnumerable<Product>>> GetLevel(int level = 0);

    Task<ApiResponse<IEnumerable<Product>>> GetChildren(Guid parentId, int level = 0);

    Task<ApiResponse<IEnumerable<Product>>> SearchByName(string name);
}
