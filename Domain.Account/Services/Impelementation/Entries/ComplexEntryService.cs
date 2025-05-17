using System.Numerics;
using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries;
using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Dtos.Entry;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.CostCenters;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces.Entries;
using Domain.Account.Utility;
using Mapster;
using Serilog;
using Shared.Responses;

namespace Domain.Account.Services.Impelementation.Entries;

public class ComplexEntryService : BaseService<Entry, ComplexEntryCreateCommand, ComplexEntryUpdateCommand>, IComplexEntryService
{
    private ApplicationDbContext _dbContext;
    private IEntryService _entryService;
    public ComplexEntryService(IEntryRepository repository, ApplicationDbContext dbContext, IEntryService entryService) : base(repository)
    {
        _dbContext = dbContext;
        _entryService = entryService;
    }

    public override async Task<ApiResponse<Entry>> Create(ComplexEntryCreateCommand command, bool isValidate = true)
    {
        try
        {
            var financialTransactions = new List<FinancialTransaction>();
            int i = 1;
            command.FinancialTransactions.ForEach(e => e.OrderNumber = i++);
            MapFinancialTransactions(command.FinancialTransactions, financialTransactions);

            var createEntryCommand = command.Adapt<EntryCreateCommand>();
            createEntryCommand.FinancialTransactions = financialTransactions;

            return await _entryService.Create(createEntryCommand);
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return new ApiResponse<Entry>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = [ex.Message]
            };
        }
    }
    public override async Task<ApiResponse<Entry>> Update(ComplexEntryUpdateCommand command, bool isValidate = true)
    {
        try
        {
            var financialTransactions = new List<FinancialTransaction>();
            int i = 1;
            command.FinancialTransactions.ForEach(e => e.OrderNumber = i++);
            MapFinancialTransactions(command.FinancialTransactions, financialTransactions);

            var updateEntryCommand = command.Adapt<EntryUpdateCommand>();
            updateEntryCommand.FinancialTransactions = financialTransactions;

            return await _entryService.Update(updateEntryCommand);
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return new ApiResponse<Entry>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = [ex.Message]
            };
        }
    }

    private static void MapFinancialTransactions(IEnumerable<ComplexFinancialTransactionDto> complexTransactions, List<FinancialTransaction> financialTransactions)
    {
        foreach (var transaction in complexTransactions)
        {
            var debitTransaction = transaction.Adapt<FinancialTransaction>();
            var creditTransaction = transaction.Adapt<FinancialTransaction>();
            debitTransaction.ChartOfAccountId = transaction.DebitAccountId;
            debitTransaction.Id = transaction.Id.Equals(Guid.NewGuid()) ? Guid.NewGuid() : transaction.Id;
            creditTransaction.Id = transaction.ComplementId.Equals(Guid.NewGuid()) ? Guid.NewGuid() : transaction.ComplementId;
            creditTransaction.ChartOfAccountId = transaction.CreditAccountId;
            creditTransaction.AccountNature = debitTransaction.AccountNature == AccountNature.Debit
                ? AccountNature.Credit
                : AccountNature.Debit;
            creditTransaction.ComplementTransactionId = debitTransaction.Id;
            debitTransaction.ComplementTransactionId = creditTransaction.Id;

            financialTransactions.Add(debitTransaction);
            financialTransactions.Add(creditTransaction);
        }
    }

    public async Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
    {
        var result = new EntryNumberDto();

        var financialPeriod = await _dbContext.Set<FinancialPeriod>()
            .FirstOrDefaultAsync(e => e.StartDate <= dateTime && e.EndDate > dateTime);
        if (financialPeriod == null)
        {
            return new ApiResponse<EntryNumberDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.Found,
                ErrorMessages = new List<string> { "NotFoundCurrentFinancialPeriod" }
            };
        }

        result.FinancialPeriodId = financialPeriod.Id;
        result.FinancialPeriodNumber = financialPeriod.YearNumber;


        var lastEntryNumber = _dbContext.Set<Entry>()
                                    .Where(e => e.FinancialPeriodId == financialPeriod.Id)
                                    .ToList()
                                    .OrderByDescending(e => BigInteger.Parse(e.EntryNumber))
                                    .Select(e => BigInteger.Parse(e.EntryNumber))
                                    .FirstOrDefault();
        if (lastEntryNumber == null)
            lastEntryNumber = 0;

        result.EntryNumber = (lastEntryNumber + 1).ToString();

        return new ApiResponse<EntryNumberDto>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = result
        };
    }

    public async Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id, EntryType? entryType = null)
    {
        try
        {

            var entryDto = await _dbContext.Set<Entry>().Include(e => e.FinancialTransactions).
     Include(e => e.FinancialPeriod).Include(e => e.CostCenters).Include(e => e.EntryAttachments).ThenInclude(e => e.Attachment)
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
                ErrorMessages = [ex.Message]
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries(EntryType? entryType = null)
    {
        var entries = await _dbContext.Set<Entry>().Include(e => e.FinancialTransactions).
            Include(e => e.FinancialPeriod)
            .Where(e => (entryType == null ? true : e.EntryType == entryType))
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
    private static IEnumerable<ComplexFinancialTransactionDto> CreateComplexFinancialTransactionDto(List<FinancialTransaction> financialTransactions)
    {
        foreach (var debitTransaction in financialTransactions.Where(e => e.AccountNature == AccountNature.Debit))
        {
            FinancialTransaction compelementTransaction =
                financialTransactions.FirstOrDefault(e => e.Id == debitTransaction.ComplementTransactionId);

            var dto = debitTransaction.Adapt<ComplexFinancialTransactionDto>();
            dto.DebitAccountId = debitTransaction.ChartOfAccountId;
            dto.CreditAccountId = compelementTransaction.ChartOfAccountId;
            dto.Id = debitTransaction.Id;
            dto.ComplementId = compelementTransaction.Id;
            yield return dto;
        }
    }
    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(ComplexEntryCreateCommand command)
    {
        var validationResult = await base.ValidateCreate(command);

        var financialPeriod = await _dbContext.Set<FinancialPeriod>()
            .FirstOrDefaultAsync(e => e.StartDate <= command.EntryDate && e.EndDate > command.EntryDate);
        if (financialPeriod == null || command.FinancialPeriodId != financialPeriod.Id)
        {
            validationResult.isValid = false;
            validationResult.errors.Add("NotFoundCurrentFinancialPeriod");
        }

        var entryWithSameNumber = await _dbContext.Set<Entry>()
            .Where(e => e.EntryNumber == command.EntryNumber && e.FinancialPeriodId == command.FinancialPeriodId)
            .FirstOrDefaultAsync();
        if (entryWithSameNumber != null)
        {
            validationResult.isValid = false;
            validationResult.errors.Add("ExistedEntryWithSameNumber");
        }

        return validationResult;
    }


}