using Accounting.Contracts.Dtos;
using Accounting.Application.Dtos.Entries;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.DTOs;
using System.Linq.Expressions;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Entry read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public class EntryService(
    IEntryRepository repository,
    IUnitOfWork unitOfWork,
    IFinancialPeriodRepository financialPeriodRepository) : IEntryService
{
    public async Task<ApiResponse<IEnumerable<EntryDto>>> Get(EntryType? entryType = null)
    {
        var entries = await repository.GetQuery().Include(e => e.FinancialTransactions).Include(e => e.FinancialPeriod)
            .Where(e => entryType == null ? true : e.EntryType == entryType)
            .Select(e => new EntryDto
            {
                Id = e.Id,
                EntryDate = e.EntryDate,
                BranchId = e.BranchId,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                CurrencyId = e.CurrencyId,
                DocumentNumber = e.DocumentNumber,
                EntryNumber = e.EntryNumber,
                ExchangeRate = e.ExchangeRate,
                FinancialPeriodId = e.FinancialPeriodId,
                FinancialPeriodNumber = e.FinancialPeriod != null ? e.FinancialPeriod.YearNumber : string.Empty,
                FinancialTransactions = e.FinancialTransactions,
                ModifiedAt = e.ModifiedAt,
                ModifiedBy = e.ModifiedBy,
                Notes = e.Notes,
                ReceiverName = e.ReceiverName,
            }).ToListAsync();
        if (entries == null)
        {
            return new ApiResponse<IEnumerable<EntryDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
            };
        }

        return new ApiResponse<IEnumerable<EntryDto>>
        {
            IsSuccess = true,
            Result = entries,
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task<ApiResponse<EntryDto>> Get(Guid id, EntryType? entryType = null)
    {
        var entry = await repository.Get(id, entryType ?? EntryType.Combined);
        if (entry == null)
        {
            return new ApiResponse<EntryDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
            };
        }

        var financialPeriod = await financialPeriodRepository.Get(entry.FinancialPeriodId);

        var entryDto = entry.Adapt<EntryDto>();
        entryDto.FinancialPeriodNumber = financialPeriod?.YearNumber;

        return new ApiResponse<EntryDto>
        {
            IsSuccess = true,
            Result = entryDto,
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task<ApiResponse<IEnumerable<Entry>>> ReadAll()
    {
        try
        {
            var entities = await repository.Get();
            return new ApiResponse<IEnumerable<Entry>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<Entry>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Expression<Func<Entry, bool>>? filterExpression = null;

            if (filter.CreatedFrom.HasValue || filter.CreatedTo.HasValue)
            {
                filterExpression = e =>
                    (!filter.CreatedFrom.HasValue || e.CreatedAt >= filter.CreatedFrom.Value) &&
                    (!filter.CreatedTo.HasValue || e.CreatedAt <= filter.CreatedTo.Value);
            }

            Expression<Func<Entry, object>>? orderBy = string.IsNullOrEmpty(filter.SortBy)
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

            return new ApiResponse<PaginatedResult<Entry>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PaginatedResult<Entry>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
    {
        var result = new EntryNumberDto();

        var financialPeriod = await unitOfWork.Set<FinancialPeriod>()
            .FirstOrDefaultAsync(e => e.StartDate <= dateTime && e.EndDate > dateTime);
        if (financialPeriod == null)
        {
            return new ApiResponse<EntryNumberDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.Found,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.FinancialPeriodNotFound }]
            };
        }

        result.FinancialPeriodId = financialPeriod.Id;
        result.FinancialPeriodNumber = financialPeriod.YearNumber ?? string.Empty;

        var lastEntryNumber = unitOfWork.Set<Entry>()
            .Where(e => e.FinancialPeriodId == financialPeriod.Id)
            .ToList()
            .OrderByDescending(e => BigInteger.Parse(e.EntryNumber))
            .Select(e => BigInteger.Parse(e.EntryNumber))
            .FirstOrDefault();

        result.EntryNumber = lastEntryNumber == 0
            ? "1"
            : (lastEntryNumber + 1).ToString();

        return new ApiResponse<EntryNumberDto>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = result
        };
    }
}
