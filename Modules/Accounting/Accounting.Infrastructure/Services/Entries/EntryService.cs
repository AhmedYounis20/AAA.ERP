using Accounting.Contracts.Dtos;
using Accounting.Application.Dtos.Entries;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using Accounting.Infrastructure.Services.Entries;
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
            var filterExpression = EntryFilterSupport.BuildFilterExpression(filter);

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

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => EntryNumberSupport.GetNextEntryNumberAsync(
            unitOfWork.Set<FinancialPeriod>(),
            unitOfWork.Set<Entry>(),
            dateTime);
}
