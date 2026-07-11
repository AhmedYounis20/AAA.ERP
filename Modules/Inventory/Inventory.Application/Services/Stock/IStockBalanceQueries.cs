using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.DTOs.Filters;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos.Catalog;
using Inventory.Domain.Models.Entities.StockBalances;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for stock balance GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IStockBalanceQueries
{
    Task<ApiResponse<IEnumerable<StockBalanceReadDto>>> GetAll();

    Task<ApiResponse<PaginatedResult<StockBalance>>> GetPaginated(
        StockBalanceFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<StockBalanceReadDto>> GetById(Guid id);

    Task<ApiResponse<StockBalanceReadDto>> GetByVariantAndBranch(Guid variantId, Guid branchId);

    Task<ApiResponse<StockBalanceReadDto>> GetByVariantPackingUnitAndBranch(
        Guid variantId,
        Guid packingUnitId,
        Guid branchId);

    Task<ApiResponse<IEnumerable<StockBalanceReadDto>>> GetByBranch(Guid branchId);

    Task<ApiResponse<IEnumerable<StockBalanceReadDto>>> GetByVariant(Guid variantId);

    Task<ApiResponse<decimal>> GetCurrentBalance(Guid variantId, Guid packingUnitId, Guid branchId);
}
