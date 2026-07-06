using Accounting.Contracts.Dtos;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.DTOs.Filters;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos.Catalog;
using Inventory.Application.Services;
using Inventory.Application.Services.Transactions;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Services.Transactions;

/// <summary>
/// Import transaction reads. Transitional: delegates to <see cref="IImportTransactionService"/>.
/// </summary>
public class ImportTransactionQueries(
    IImportTransactionService importTransactionService,
    IInventoryTransactionService inventoryTransactionService) : IImportTransactionQueries
{
    public Task<ApiResponse<IEnumerable<InventoryTransactionReadDto>>> GetAll()
        => importTransactionService.GetAll();

    public Task<ApiResponse<PaginatedResult<InventoryTransaction>>> GetPaginated(
        InventoryTransactionFilterDto filter,
        CancellationToken cancellationToken = default)
        => inventoryTransactionService.GetPaginated(filter, cancellationToken);

    public Task<ApiResponse<InventoryTransactionReadDto>> GetById(Guid id)
        => importTransactionService.GetById(id);

    public Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime)
        => importTransactionService.GetTransactionNumber(dateTime);
}
