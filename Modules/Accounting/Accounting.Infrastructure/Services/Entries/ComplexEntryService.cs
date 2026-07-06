using Accounting.Contracts.Dtos;
using Accounting.Application.Dtos.Entries;
using Accounting.Application.Services;
using Accounting.Application.Commands.Entries;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Complex entry read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public class ComplexEntryService(IApplicationDbContext dbContext, IEntryService entryService) : IComplexEntryService
{
    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => entryService.GetEntryNumber(dateTime);

    public async Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id, EntryType? entryType = null)
    {
        try
        {
            var entryDto = await dbContext.Set<Entry>().Include(e => e.FinancialTransactions)
                .Include(e => e.FinancialPeriod).Include(e => e.CostCenters).Include(e => e.EntryAttachments)
                .ThenInclude(e => e.Attachment)
                .Where(e => e.Id == id && (entryType == null || e.EntryType == entryType))
                .Select(e => new ComplexEntryDto
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
                    FinancialPeriod = e.FinancialPeriod,
                    FinancialPeriodNumber = e.FinancialPeriod == null ? string.Empty : e.FinancialPeriod.YearNumber,
                    FinancialTransactions = CreateComplexFinancialTransactionDto(e.FinancialTransactions).ToList(),
                    CostCenters = e.CostCenters.ToList(),
                    ModifiedAt = e.ModifiedAt,
                    ModifiedBy = e.ModifiedBy,
                    Notes = e.Notes,
                    ReceiverName = e.ReceiverName,
                    Attachments = e.EntryAttachments.Select(e => e.Attachment).ToAttachmentDto().ToList(),
                }).FirstOrDefaultAsync();

            return new ApiResponse<ComplexEntryDto>
            {
                IsSuccess = true,
                Result = entryDto,
                StatusCode = HttpStatusCode.OK
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ComplexEntryDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries(EntryType? entryType = null)
    {
        var entries = await dbContext.Set<Entry>().Include(e => e.FinancialTransactions)
            .Include(e => e.FinancialPeriod)
            .Where(e => entryType == null ? true : e.EntryType == entryType)
            .Select(e => new ComplexEntryDto
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
                FinancialTransactions = CreateComplexFinancialTransactionDto(e.FinancialTransactions).ToList(),
                ModifiedAt = e.ModifiedAt,
                ModifiedBy = e.ModifiedBy,
                Notes = e.Notes,
                ReceiverName = e.ReceiverName,
            }).ToListAsync();

        if (entries == null)
        {
            return new ApiResponse<IEnumerable<ComplexEntryDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
            };
        }

        return new ApiResponse<IEnumerable<ComplexEntryDto>>
        {
            IsSuccess = true,
            Result = entries,
            StatusCode = HttpStatusCode.OK
        };
    }

    private static IEnumerable<ComplexFinancialTransactionDto> CreateComplexFinancialTransactionDto(
        List<FinancialTransaction> financialTransactions)
    {
        foreach (var debitTransaction in financialTransactions.Where(e => e.AccountNature == AccountNature.Debit))
        {
            var compelementTransaction =
                financialTransactions.FirstOrDefault(e => e.Id == debitTransaction.ComplementTransactionId);

            var dto = debitTransaction.Adapt<ComplexFinancialTransactionDto>();
            dto.DebitAccountId = debitTransaction.ChartOfAccountId;
            dto.CreditAccountId = compelementTransaction.ChartOfAccountId;
            dto.Id = debitTransaction.Id;
            dto.ComplementId = compelementTransaction.Id;
            yield return dto;
        }
    }
}
