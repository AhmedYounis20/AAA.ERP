using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.DTOs.Filters;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos.Catalog;
using Inventory.Application.Services;
using Inventory.Application.Services.Transactions;
using Inventory.Domain.Models.Entities;

namespace Inventory.Infrastructure.Services.Transactions;

/// <summary>
/// Inventory transfer reads. Transitional: delegates to <see cref="IInventoryTransferService"/>.
/// </summary>
public class InventoryTransferQueries(IInventoryTransferService transferService) : IInventoryTransferQueries
{
    public Task<ApiResponse<IEnumerable<InventoryTransfer>>> GetAll()
        => transferService.ReadAll();

    public Task<ApiResponse<PaginatedResult<InventoryTransfer>>> GetPaginated(
        InventoryTransferFilterDto filter,
        CancellationToken cancellationToken = default)
        => transferService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<InventoryTransferReadDto>> GetWithDetails(Guid id)
        => transferService.GetWithDetails(id);

    public Task<ApiResponse<IEnumerable<InventoryTransferReadDto>>> GetByStatus(InventoryTransferStatus status)
        => transferService.GetByStatus(status);
}
