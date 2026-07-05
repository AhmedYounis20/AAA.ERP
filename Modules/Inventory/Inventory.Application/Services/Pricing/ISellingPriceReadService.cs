using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for selling price GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ISellingPriceReadService
{
    Task<ApiResponse<IEnumerable<SellingPriceDto>>> GetDtos();

    Task<ApiResponse<PaginatedResult<SellingPrice>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<SellingPrice>> GetById(Guid id);
}
