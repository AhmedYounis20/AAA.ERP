using System.Numerics;
using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Currencies;
using Domain.Account.Commands.GLSettings;
using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.Attachments;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Models.Entities.GLSettings;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces;
using Mapster;
using Serilog;
using Shared.Responses;

namespace Domain.Account.Services.Impelementation;

public class EntryService : BaseService<Entry,EntryCreateCommand,EntryUpdateCommand>,IEntryService
{
    private ApplicationDbContext _dbContext;
    private IEntryRepository _repository;
    public EntryService(IEntryRepository repository,ApplicationDbContext dbContext) : base(repository)
    {
        _dbContext = dbContext;
        _repository = repository;
    }

       public override async Task<ApiResponse<Entry>> Create(EntryCreateCommand command, bool isValidate = true)
    {
        try
        {
            var entry = command.Adapt<Entry>();
            if (command.Attachments != null && command.Attachments.Any())
            {
                foreach (var file in command.Attachments)
                {
                    if (file != null)
                    {
                        Attachment attachment = new();
                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);
                        attachment = new Attachment
                        {
                            FileData = memoryStream.ToArray(),
                            FileContentType = file.ContentType,
                            FileName = file.FileName
                        };
                        entry.EntryAttachments.Add(new EntryAttachment
                        {
                            Attachment = attachment
                        });
                    }
                }
            }

            entry = await _repository.Add(entry);
           return new ApiResponse<Entry>
           {
               IsSuccess = true,
               StatusCode = HttpStatusCode.OK,
               Result = entry
           };
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return new ApiResponse<Entry>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = [ex.Message]
            };
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

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(EntryCreateCommand command)
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