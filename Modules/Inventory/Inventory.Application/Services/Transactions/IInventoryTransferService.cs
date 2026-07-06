using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.DTOs.Filters;
using Inventory.Application.Dtos.Catalog;
using Inventory.Domain.Models.Entities;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.Application.Services;

/// <summary>
/// Inventory transfer read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public interface IInventoryTransferService
{
    Task<ApiResponse<InventoryTransferReadDto>> GetWithDetails(Guid id);

    Task<ApiResponse<IEnumerable<InventoryTransferReadDto>>> GetByStatus(InventoryTransferStatus status);

    Task<ApiResponse<IEnumerable<InventoryTransfer>>> ReadAll();

    Task<ApiResponse<PaginatedResult<InventoryTransfer>>> GetPaginated(
        InventoryTransferFilterDto filter,
        CancellationToken cancellationToken = default);
}
