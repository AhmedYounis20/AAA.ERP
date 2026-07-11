using Accounting.Contracts.Integration;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.DTOs.Filters;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos.Catalog;
using Inventory.Application.Repositories;
using Inventory.Application.Services.Transactions;
using Inventory.Domain.Models.Entities;
using Inventory.Infrastructure.Mapping;
using System.Linq.Expressions;

namespace Inventory.Infrastructure.Services.Transactions;

/// <summary>
/// Inventory transfer reads.
/// </summary>
public class InventoryTransferQueries(
    IInventoryTransferRepository repository,
    IInventoryAccountingDisplayQueries displayQueries) : IInventoryTransferQueries
{
    public async Task<ApiResponse<InventoryTransferReadDto>> GetWithDetails(Guid id)
    {
        var transfer = await repository.GetWithDetails(id);
        if (transfer == null)
        {
            return new ApiResponse<InventoryTransferReadDto>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.TransferNotFound }]
            };
        }

        var dto = await InventoryReadDtoMapper.ToReadDtoAsync(transfer, displayQueries);
        return new ApiResponse<InventoryTransferReadDto> { IsSuccess = true, Result = dto };
    }

    public async Task<ApiResponse<IEnumerable<InventoryTransferReadDto>>> GetByStatus(InventoryTransferStatus status)
    {
        var transfers = await repository.GetByStatus(status);
        var dtos = await InventoryReadDtoMapper.ToReadDtosAsync(transfers, displayQueries);
        return new ApiResponse<IEnumerable<InventoryTransferReadDto>> { IsSuccess = true, Result = dtos };
    }

    public async Task<ApiResponse<IEnumerable<InventoryTransfer>>> GetAll()
    {
        try
        {
            var entities = await repository.Get();
            return new ApiResponse<IEnumerable<InventoryTransfer>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<InventoryTransfer>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<PaginatedResult<InventoryTransfer>>> GetPaginated(
        InventoryTransferFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Expression<Func<InventoryTransfer, bool>>? filterExpression = null;

            if (filter.CreatedFrom.HasValue || filter.CreatedTo.HasValue)
            {
                filterExpression = e =>
                    (!filter.CreatedFrom.HasValue || e.CreatedAt >= filter.CreatedFrom.Value) &&
                    (!filter.CreatedTo.HasValue || e.CreatedAt <= filter.CreatedTo.Value);
            }

            Expression<Func<InventoryTransfer, object>>? orderBy = string.IsNullOrEmpty(filter.SortBy)
                ? e => e.CreatedAt
                : filter.SortBy.ToLower() switch
                {
                    "createdat" => e => e.CreatedAt,
                    "modifiedat" => e => e.ModifiedAt,
                    _ => e => e.CreatedAt
                };

            var result = await repository.GetPaginated(
                filter.PageNumber,
                filter.PageSize,
                filterExpression,
                orderBy,
                filter.SortDescending,
                cancellationToken);

            return new ApiResponse<PaginatedResult<InventoryTransfer>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PaginatedResult<InventoryTransfer>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }
}
