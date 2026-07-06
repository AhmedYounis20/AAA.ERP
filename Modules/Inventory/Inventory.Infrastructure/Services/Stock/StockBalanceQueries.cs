using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.DTOs.Filters;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos.Catalog;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Sizes;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Stock balance read queries.
/// </summary>
public class StockBalanceQueries(
    IStockBalanceRepository repository,
    IStockBalanceService stockBalanceService) : IStockBalanceQueries
{
    private readonly BaseEntityRepositoryQueries<StockBalance> _entityQueries = new(repository);

    public Task<ApiResponse<IEnumerable<StockBalanceReadDto>>> GetAll()
        => stockBalanceService.GetAllForDisplay();

    public Task<ApiResponse<PaginatedResult<StockBalance>>> GetPaginated(
        StockBalanceFilterDto filter,
        CancellationToken cancellationToken = default)
        => _entityQueries.GetPaginated(filter, cancellationToken);

    public Task<ApiResponse<StockBalanceReadDto>> GetById(Guid id)
        => stockBalanceService.GetDisplayById(id);

    public Task<ApiResponse<StockBalanceReadDto>> GetByVariantAndBranch(Guid variantId, Guid branchId)
        => stockBalanceService.GetByVariantAndBranch(variantId, branchId);

    public Task<ApiResponse<StockBalanceReadDto>> GetByVariantPackingUnitAndBranch(
        Guid variantId,
        Guid packingUnitId,
        Guid branchId)
        => stockBalanceService.GetByVariantPackingUnitAndBranch(variantId, packingUnitId, branchId);

    public Task<ApiResponse<IEnumerable<StockBalanceReadDto>>> GetByBranch(Guid branchId)
        => stockBalanceService.GetByBranch(branchId);

    public Task<ApiResponse<IEnumerable<StockBalanceReadDto>>> GetByVariant(Guid variantId)
        => stockBalanceService.GetByVariant(variantId);

    public Task<ApiResponse<decimal>> GetCurrentBalance(Guid variantId, Guid packingUnitId, Guid branchId)
        => stockBalanceService.GetCurrentBalance(variantId, packingUnitId, branchId);
}
