using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.Products;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for product GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IProductQueries : ITreeSettingQueries<Product>
{
    new Task<ApiResponse<List<ProductDto>>> GetAll();

    new Task<ApiResponse<ProductDto?>> GetById(Guid id);

    Task<ApiResponse<string>> GetNextCode(Guid? parentId = null);
}
