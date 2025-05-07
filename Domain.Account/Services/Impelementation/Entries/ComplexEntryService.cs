using System.Numerics;
using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries;
using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Dtos.Entry;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces;
using Domain.Account.Services.Interfaces.Entries;
using Domain.Account.Utility;
using Mapster;
using Serilog;
using Shared.Responses;

namespace Domain.Account.Services.Impelementation.Entries;

public class ComplexEntryService : BaseService<Entry,ComplexEntryCreateCommand,ComplexEntryUpdateCommand>,IComplexEntryService
{
    private ApplicationDbContext _dbContext;
    private IEntryRepository _repository;
    private IEntryService _entryService;
    public ComplexEntryService(IEntryRepository repository,ApplicationDbContext dbContext, IEntryService entryService) : base(repository)
    {
        _dbContext = dbContext;
        _repository = repository;
        _entryService = entryService;
    }

    public override async Task<ApiResponse<Entry>> Create(ComplexEntryCreateCommand command, bool isValidate = true)
    {
        try
        {
            var financialTransactions = new List<FinancialTransaction>();
            int i = 1;
            command.FinancialTransactions.ForEach(e=>e.OrderNumber = i++);
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
            command.FinancialTransactions.ForEach(e=>e.OrderNumber = i++);
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
                ErrorMessages = new List<string> {"NotFoundCurrentFinancialPeriod"}
            };
        }

        result.FinancialPeriodId = financialPeriod.Id;
        result.FinancialPeriodNumber = financialPeriod.YearNumber;


        var lastEntryNumber =  _dbContext.Set<Entry>()
                                    .Where(e => e.FinancialPeriodId == financialPeriod.Id)
                                    .ToList()
                                    .OrderByDescending(e =>BigInteger.Parse(e.EntryNumber))
                                    .Select(e =>BigInteger.Parse(e.EntryNumber))
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

    public async Task<ApiResponse<EntryDto>> GetComplexEntryById(Guid id)
    {
        var entry = await _dbContext.Set<Entry>().Include(e=>e.FinancialTransactions).FirstOrDefaultAsync(e => e.Id == id);
        if (entry == null)
        {
            return new ApiResponse<EntryDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
            };
        }

        var financialPeriod = await _dbContext.Set<FinancialPeriod>()
            .FirstOrDefaultAsync(e => e.Id == entry.FinancialPeriodId);
        
        var entryDto = entry.Adapt<EntryDto>();
        entryDto.FinancialPeriodNumber = financialPeriod?.YearNumber;
        entryDto.Attachments = (await _dbContext.Set<EntryAttachment>().Include(e => e.Attachment)
            .Where(e => e.EntryId == id)
            .Select(e => e.Attachment)
            .ToListAsync()).ToAttachmentDto().ToList();

        entryDto.FinancialTransactions = CreateComplexFinancialTransactionDto(entry.FinancialTransactions).ToList();
        return new ApiResponse<EntryDto>
        {
            IsSuccess = true,
            Result = entryDto,
            StatusCode = HttpStatusCode.OK
        };
    }
    private  IEnumerable<ComplexFinancialTransactionDto> CreateComplexFinancialTransactionDto(List<FinancialTransaction> financialTransactions)
    {
        foreach (var debitTransaction in financialTransactions.Where(e=>e.AccountNature == AccountNature.Debit))
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
       var validationResult =  await base.ValidateCreate(command);
        
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