using Accounting.Contracts.Integration;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Application.Dtos.Catalog;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using Inventory.Infrastructure.Mapping;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Export transaction read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public class ExportTransactionService(
    IInventoryTransactionService inventoryTransactionService,
    IInventoryTransactionRepository repository,
    IInventoryAccountingDisplayQueries displayQueries) : IExportTransactionService
{
    public async Task<ApiResponse<InventoryTransactionReadDto>> GetById(Guid id)
    {
        var transaction = await repository.GetWithItems(id);
        if (transaction == null)
        {
            return new ApiResponse<InventoryTransactionReadDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.TransactionNotFound }]
            };
        }

        if (transaction.TransactionType != InventoryTransactionType.Issue)
        {
            return new ApiResponse<InventoryTransactionReadDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.TransactionIsNotExportTransaction }]
            };
        }

        var dto = await InventoryReadDtoMapper.ToReadDtoAsync(transaction, displayQueries);
        return new ApiResponse<InventoryTransactionReadDto>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = dto
        };
    }

    public async Task<ApiResponse<IEnumerable<InventoryTransactionReadDto>>> GetAll()
    {
        var result = await inventoryTransactionService.GetByTransactionType(InventoryTransactionType.Issue);
        if (!result.IsSuccess || result.Result is null)
        {
            return new ApiResponse<IEnumerable<InventoryTransactionReadDto>>
            {
                IsSuccess = false,
                StatusCode = result.StatusCode,
                Errors = result.Errors
            };
        }

        var dtos = await InventoryReadDtoMapper.ToReadDtosAsync(result.Result, displayQueries);
        return new ApiResponse<IEnumerable<InventoryTransactionReadDto>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = dtos
        };
    }

    public Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime)
        => inventoryTransactionService.GetTransactionNumber(dateTime);
}
