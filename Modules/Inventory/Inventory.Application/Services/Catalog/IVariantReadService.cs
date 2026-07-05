using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for variant GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IVariantReadService
{
    Task<ApiResponse<List<VariantDto>>> GetAll();

    Task<ApiResponse<PaginatedResult<VariantDto>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<VariantDto?>> GetById(Guid id);

    Task<ApiResponse<IEnumerable<VariantDto>>> GetVariantsByProductId(Guid productId);

    Task<ApiResponse<IEnumerable<VariantDto>>> SearchVariants(string searchTerm);
}
