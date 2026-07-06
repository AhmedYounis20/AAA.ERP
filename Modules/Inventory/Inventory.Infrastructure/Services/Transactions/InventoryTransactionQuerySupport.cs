using Accounting.Contracts.Dtos;
using Accounting.Contracts.Integration;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.DTOs.Filters;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using System.Linq.Expressions;
using System.Numerics;

namespace Inventory.Infrastructure.Services.Transactions;

/// <summary>
/// Shared inventory transaction read helpers for import/export query types.
/// </summary>
internal static class InventoryTransactionQuerySupport
{
    public static async Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetByTransactionType(
        IInventoryTransactionRepository repository,
        InventoryTransactionType transactionType)
    {
        try
        {
            var transactions = await repository.GetByTransactionType(transactionType);
            return new ApiResponse<IEnumerable<InventoryTransaction>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = transactions
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<InventoryTransaction>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public static async Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(
        IInventoryTransactionRepository repository,
        IAccountingIntegrationService accountingIntegration,
        DateTime dateTime)
    {
        try
        {
            var periodResult = await accountingIntegration.GetOpenPeriodForDateAsync(dateTime);
            if (!periodResult.IsSuccess || periodResult.Result is null)
            {
                return new ApiResponse<TransactionNumberDto>
                {
                    IsSuccess = false,
                    StatusCode = periodResult.StatusCode,
                    Errors = periodResult.Errors
                };
            }

            var financialPeriod = periodResult.Result;
            var result = new TransactionNumberDto
            {
                FinancialPeriodId = financialPeriod.Id,
                FinancialPeriodNumber = financialPeriod.YearNumber ?? string.Empty
            };

            var lastTransactionNumber = repository.GetQuery()
                .Where(e => e.FinancialPeriodId == financialPeriod.Id)
                .ToList()
                .OrderByDescending(e => BigInteger.Parse(e.TransactionNumber))
                .Select(e => BigInteger.Parse(e.TransactionNumber))
                .FirstOrDefault();

            result.TransactionNumber = lastTransactionNumber == 0
                ? "1"
                : (lastTransactionNumber + 1).ToString();

            return new ApiResponse<TransactionNumberDto>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<TransactionNumberDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors =
                [
                    new MessageTemplate
                    {
                        MessageKey = $"Failed to generate transaction number: {ex.Message}"
                    }
                ]
            };
        }
    }

    public static async Task<ApiResponse<PaginatedResult<InventoryTransaction>>> GetPaginated(
        IInventoryTransactionRepository repository,
        InventoryTransactionFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Expression<Func<InventoryTransaction, bool>>? filterExpression = null;

            if (filter.CreatedFrom.HasValue || filter.CreatedTo.HasValue)
            {
                filterExpression = e =>
                    (!filter.CreatedFrom.HasValue || e.CreatedAt >= filter.CreatedFrom.Value) &&
                    (!filter.CreatedTo.HasValue || e.CreatedAt <= filter.CreatedTo.Value);
            }

            Expression<Func<InventoryTransaction, object>>? orderBy = string.IsNullOrEmpty(filter.SortBy)
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

            return new ApiResponse<PaginatedResult<InventoryTransaction>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PaginatedResult<InventoryTransaction>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }
}
