using Domain.Account.Commands.Entries;
using Domain.Account.Models.Dtos.Attachments;
using Domain.Account.Models.Entities.Attachments;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Models.Entities.FinancialPeriods;
using ERP.Application.Services.Account.Entries;

namespace ERP.Infrastracture.Services.Account.Entries;

public class EntryService : BaseService<Entry, EntryCreateCommand, EntryUpdateCommand>, IEntryService
{
    private ApplicationDbContext _dbContext;
    private IEntryRepository _repository;
    public EntryService(IEntryRepository repository, ApplicationDbContext dbContext) : base(repository)
    {
        _dbContext = dbContext;
        _repository = repository;
    }

    public override async Task<ApiResponse<Entry>> Create(EntryCreateCommand command, bool isValidate = true)
    {
        try
        {
            var bussinessValidationResult = await ValidateCreate(command);
            if (!bussinessValidationResult.isValid)
            {
                return new ApiResponse<Entry>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = bussinessValidationResult.errors
                };
            }
            var entry = command.Adapt<Entry>();
            entry.EntryType = command.Type;
            entry.FinancialTransactions = command.FinancialTransactions;

            if (command.Attachments != null && command.Attachments.Any())
                entry.EntryAttachments = command.Attachments.ToAttachment().Select(e => CreateEntryAttachment(e)).ToList();

            entry = await _repository.Add(entry);
            entry.FinancialTransactions = [];

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

    public override async Task<ApiResponse<Entry>> Update(EntryUpdateCommand command, bool isValidate = true)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var bussinessValidationResult = await ValidateUpdate(command);
            if (!bussinessValidationResult.isValid || bussinessValidationResult.entity is null)
            {
                return new ApiResponse<Entry>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = bussinessValidationResult.errors
                };
            }

            Entry entry = bussinessValidationResult.entity;

            entry.EntryDate = command.EntryDate;
            entry.BranchId = command.BranchId;
            entry.ExchangeRate = command.ExchangeRate;
            entry.Notes = command.Notes;
            entry.CurrencyId = command.CurrencyId;
            await UpdateEntryAttachments(command, entry);
            await UpdateFinancialTransactions(entry, command.FinancialTransactions);
            await UpdateEntryCostCenter(entry, command.CostCenters);
            entry.FinancialTransactions = [];
            entry.EntryAttachments = [];
            _dbContext.Set<Entry>().Update(entry);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            entry.FinancialTransactions = [];
            return new ApiResponse<Entry>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entry
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Log.Error(ex.ToString());
            return new ApiResponse<Entry>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = [ex.Message]
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<EntryDto>>> Get(EntryType? entryType = null)
    {
        var entries = await _dbContext.Set<Entry>().Include(e => e.FinancialTransactions).Include(e => e.FinancialPeriod).Where(e => entryType == null ? true : e.EntryType == entryType)
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
        var entry = await _repository.Get(id, entryType ?? EntryType.Compined);
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

        EntryDto entryDto = entry.Adapt<EntryDto>();
        entryDto.FinancialPeriodNumber = financialPeriod?.YearNumber;

        return new ApiResponse<EntryDto>
        {
            IsSuccess = true,
            Result = entryDto,
            StatusCode = HttpStatusCode.OK
        };
    }

    private async Task UpdateEntryAttachments(EntryUpdateCommand command, Entry entry)
    {
        IEnumerable<EntryAttachment> oldAttachments = await _dbContext.Set<EntryAttachment>()
                                                                .Include(e => e.Attachment)
                                                                .Where(e => e.EntryId == entry.Id).ToListAsync();

        IEnumerable<EntryAttachment> deletedEntryAttachments = oldAttachments.Where(e => !command.Attachments.Any(file => file.AttachmentId == e.Attachment.Id));
        IEnumerable<Attachment> deletedAttachments = oldAttachments.Select(e => e.Attachment);
        IEnumerable<Attachment> newAttachments = command.Attachments.Where(file => !oldAttachments.Any(e => file.AttachmentId == e.Attachment.Id))
                                                                     .ToAttachment();
        List<Attachment> updatedAttachments = oldAttachments.Select(e => e.Attachment).Except(deletedAttachments)
                                                            .ToList();


        SyncUpdatedEntryAttachments(updatedAttachments, command.Attachments);
        if (deletedAttachments.Any())
        {
            _dbContext.Set<EntryAttachment>().RemoveRange(deletedEntryAttachments);
            _dbContext.Set<Attachment>().RemoveRange(deletedAttachments);
        }

        var newEntryAttachments = newAttachments.Select(e =>
        {
            var entryAttachment = CreateEntryAttachment(e);
            entryAttachment.EntryId = entry.Id;
            return entryAttachment;
        });

        if (newAttachments.Any())
            await _dbContext.Set<EntryAttachment>().AddRangeAsync(newEntryAttachments);

        if (updatedAttachments.Any())
            _dbContext.Set<Attachment>().UpdateRange(updatedAttachments);
        await _dbContext.SaveChangesAsync();
    }

    private EntryAttachment CreateEntryAttachment(Attachment attachment)
        => new EntryAttachment
        {
            Attachment = attachment
        };

    private void SyncUpdatedEntryAttachments(List<Attachment> oldAttachments,
        List<AttachmentDto> newAttachments)
    {
        foreach (var oldAttachment in oldAttachments)
        {
            AttachmentDto newAttachment = newAttachments.First(e => e.AttachmentId == oldAttachment.Id);
            newAttachment.CopyTo(oldAttachment);
        }
    }

    private async Task UpdateFinancialTransactions(Entry entry, IEnumerable<FinancialTransaction> newFinancialTransactions)
    {
        try
        {

            List<FinancialTransaction> existedFinancialTransactions =
                _dbContext.Set<FinancialTransaction>().Where(e => e.EntryId.Equals(entry.Id)).ToList();

            IEnumerable<FinancialTransaction> deletedFinancialTransactions =
                existedFinancialTransactions.Where(f => !newFinancialTransactions.Any(e => e.Id == f.Id));

            List<FinancialTransaction> addedFinancialTransactions =
                newFinancialTransactions.Where(f => !existedFinancialTransactions.Any(e => e.Id == f.Id)).ToList();
            addedFinancialTransactions.ForEach(e => e.EntryId = entry.Id);
            List<FinancialTransaction> updatedFinancialTransactions = newFinancialTransactions
                .Where(f => existedFinancialTransactions.Any(e => e.Id == f.Id)).ToList();
            SyncCreationInfoToUpdatedTransactions(existedFinancialTransactions, updatedFinancialTransactions);

            if (addedFinancialTransactions.Any())
                await _dbContext.Set<FinancialTransaction>().AddRangeAsync(addedFinancialTransactions);
            if (updatedFinancialTransactions.Any())
                _dbContext.Set<FinancialTransaction>().UpdateRange(updatedFinancialTransactions);
            if (deletedFinancialTransactions.Any())
                _dbContext.Set<FinancialTransaction>().RemoveRange(deletedFinancialTransactions);

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Log.Error($"{ex}");
            throw;
        }
    }

    private async Task UpdateEntryCostCenter(Entry entry, IEnumerable<EntryCostCenter> newCostCenters)
    {
        try
        {

            List<EntryCostCenter> existedCostCenters =
                _dbContext.Set<EntryCostCenter>().Where(e => e.EntryId.Equals(entry.Id)).ToList();

            IEnumerable<EntryCostCenter> deletedCostCenters =
                existedCostCenters.Where(f => !newCostCenters.Any(e => e.Id == f.Id));

            List<EntryCostCenter> addedCostCenters =
                newCostCenters.Where(f => !existedCostCenters.Any(e => e.Id == f.Id)).ToList();
            addedCostCenters.ForEach(e => e.EntryId = entry.Id);
            List<EntryCostCenter> updatedFinancialTransactions = newCostCenters
                .Where(f => existedCostCenters.Any(e => e.Id == f.Id)).ToList();
            SyncCreationInfoToUpdatedCostCenters(existedCostCenters, updatedFinancialTransactions);

            if (addedCostCenters.Any())
                await _dbContext.Set<EntryCostCenter>().AddRangeAsync(addedCostCenters);
            if (updatedFinancialTransactions.Any())
                _dbContext.Set<EntryCostCenter>().UpdateRange(updatedFinancialTransactions);
            if (deletedCostCenters.Any())
                _dbContext.Set<EntryCostCenter>().RemoveRange(deletedCostCenters);

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Log.Error($"{ex}");
            throw;
        }
    }

    private static void SyncCreationInfoToUpdatedTransactions(IEnumerable<FinancialTransaction> oldFinancialTransactions,
        List<FinancialTransaction> newFinancialTransactions)
    {
        foreach (var transaction in newFinancialTransactions)
        {
            FinancialTransaction? oldTransaction = oldFinancialTransactions.FirstOrDefault(e => e.Id == transaction.Id);
            if (oldTransaction != null)
            {

                transaction.CreatedAt = oldTransaction.CreatedAt;
                transaction.CreatedBy = oldTransaction.CreatedBy;
                transaction.EntryId = oldTransaction.EntryId;
            }
        }
    }

    private static void SyncCreationInfoToUpdatedCostCenters(IEnumerable<EntryCostCenter> oldCostCenters,
    List<EntryCostCenter> newCostCenters)
    {
        foreach (var costCenter in newCostCenters)
        {
            EntryCostCenter? oldCostCenter = oldCostCenters.FirstOrDefault(e => e.Id == costCenter.Id);
            if (oldCostCenter != null)
            {
                costCenter.CreatedAt = oldCostCenter.CreatedAt;
                costCenter.CreatedBy = oldCostCenter.CreatedBy;
                costCenter.EntryId = oldCostCenter.EntryId;
            }
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
        result.FinancialPeriodNumber = financialPeriod.YearNumber ?? string.Empty;


        var lastEntryNumber = _dbContext.Set<Entry>()
                                    .Where(e => e.FinancialPeriodId == financialPeriod.Id)
                                    .ToList()
                                    .OrderByDescending(e => BigInteger.Parse(e.EntryNumber))
                                    .Select(e => BigInteger.Parse(e.EntryNumber))
                                    .FirstOrDefault();

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

    protected override Task<(bool isValid, List<string> errors, Entry? entity)> ValidateUpdate(EntryUpdateCommand command)
    {
        return base.ValidateUpdate(command);
    }
}