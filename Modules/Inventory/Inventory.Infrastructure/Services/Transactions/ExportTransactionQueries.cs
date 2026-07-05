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
/// Export transaction reads. Transitional: delegates to <see cref="IExportTransactionService"/>.
/// </summary>
public class ExportTransactionQueries(IExportTransactionService exportTransactionService) : IExportTransactionQueries
{
    public Task<ApiResponse<IEnumerable<InventoryTransactionReadDto>>> GetAll()
        => exportTransactionService.GetAll();

    public Task<ApiResponse<PaginatedResult<InventoryTransaction>>> GetPaginated(
        InventoryTransactionFilterDto filter,
        CancellationToken cancellationToken = default)
        => exportTransactionService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<InventoryTransactionReadDto>> GetById(Guid id)
        => exportTransactionService.GetById(id);

    public Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime)
        => exportTransactionService.GetTransactionNumber(dateTime);
}
