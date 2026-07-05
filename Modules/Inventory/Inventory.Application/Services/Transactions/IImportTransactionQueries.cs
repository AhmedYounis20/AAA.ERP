using Accounting.Contracts.Dtos;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.DTOs.Filters;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos.Catalog;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Application.Services.Transactions;

/// <summary>
/// Read-side contract for import transaction GET endpoints (hybrid CQRS).
/// </summary>
public interface IImportTransactionQueries
{
    Task<ApiResponse<IEnumerable<InventoryTransactionReadDto>>> GetAll();

    Task<ApiResponse<PaginatedResult<InventoryTransaction>>> GetPaginated(
        InventoryTransactionFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<InventoryTransactionReadDto>> GetById(Guid id);

    Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime);
}
