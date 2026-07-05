using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.DTOs.Filters;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos.Catalog;
using Inventory.Domain.Models.Entities;

namespace Inventory.Application.Services.Transactions;

/// <summary>
/// Read-side contract for inventory transfer GET endpoints (hybrid CQRS).
/// </summary>
public interface IInventoryTransferQueries
{
    Task<ApiResponse<IEnumerable<InventoryTransfer>>> GetAll();

    Task<ApiResponse<PaginatedResult<InventoryTransfer>>> GetPaginated(
        InventoryTransferFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<InventoryTransferReadDto>> GetWithDetails(Guid id);

    Task<ApiResponse<IEnumerable<InventoryTransferReadDto>>> GetByStatus(InventoryTransferStatus status);
}
