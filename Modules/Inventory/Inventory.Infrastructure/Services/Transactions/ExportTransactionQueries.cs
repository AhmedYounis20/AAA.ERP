using Accounting.Contracts.Dtos;
using Accounting.Contracts.Integration;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.DTOs.Filters;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos.Catalog;
using Inventory.Application.Repositories;
using Inventory.Application.Services.Transactions;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using Inventory.Infrastructure.Mapping;

namespace Inventory.Infrastructure.Services.Transactions;

/// <summary>
/// Export transaction reads.
/// </summary>
public class ExportTransactionQueries(
    IInventoryTransactionRepository repository,
    IAccountingIntegrationService accountingIntegration,
    IInventoryAccountingDisplayQueries displayQueries) : IExportTransactionQueries
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
        var result = await InventoryTransactionQuerySupport.GetByTransactionType(
            repository,
            InventoryTransactionType.Issue);
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

    public Task<ApiResponse<PaginatedResult<InventoryTransaction>>> GetPaginated(
        InventoryTransactionFilterDto filter,
        CancellationToken cancellationToken = default)
        => InventoryTransactionQuerySupport.GetPaginated(repository, filter, cancellationToken);

    public Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime)
        => InventoryTransactionQuerySupport.GetTransactionNumber(repository, accountingIntegration, dateTime);
}
