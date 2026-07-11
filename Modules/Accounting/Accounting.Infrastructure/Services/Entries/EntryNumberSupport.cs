using Accounting.Contracts.Dtos;
using Accounting.Domain.Models.Entities.Entries;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Infrastructure.Services.Entries;

internal static class EntryNumberSupport
{
    public static async Task<ApiResponse<EntryNumberDto>> GetNextEntryNumberAsync(
        IQueryable<FinancialPeriod> financialPeriods,
        IQueryable<Entry> entries,
        DateTime dateTime,
        CancellationToken cancellationToken = default)
    {
        var financialPeriod = await financialPeriods
            .AsNoTracking()
            .FirstOrDefaultAsync(
                e => e.StartDate <= dateTime && e.EndDate > dateTime,
                cancellationToken);

        if (financialPeriod is null)
        {
            return new ApiResponse<EntryNumberDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.FinancialPeriodNotFound }]
            };
        }

        var entryNumbers = await entries
            .AsNoTracking()
            .Where(e => e.FinancialPeriodId == financialPeriod.Id)
            .Select(e => e.EntryNumber)
            .ToListAsync(cancellationToken);

        var lastEntryNumber = entryNumbers
            .Select(ParseEntryNumberSuffix)
            .DefaultIfEmpty(BigInteger.Zero)
            .Max();

        return new ApiResponse<EntryNumberDto>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = new EntryNumberDto
            {
                FinancialPeriodId = financialPeriod.Id,
                FinancialPeriodNumber = financialPeriod.YearNumber ?? string.Empty,
                EntryNumber = lastEntryNumber == BigInteger.Zero
                    ? "1"
                    : (lastEntryNumber + 1).ToString()
            }
        };
    }

    public static async Task<bool> EntryNumberExistsAsync(
        IQueryable<Entry> entries,
        Guid financialPeriodId,
        string entryNumber,
        CancellationToken cancellationToken = default)
        => await entries.AnyAsync(
            e => e.FinancialPeriodId == financialPeriodId && e.EntryNumber == entryNumber,
            cancellationToken);

    private static BigInteger ParseEntryNumberSuffix(string? entryNumber)
        => BigInteger.TryParse(entryNumber?.Trim(), out var suffix) ? suffix : BigInteger.Zero;
}
