using Domain.Account.Models.Dtos.Attachments;
using ERP.Application.Repositories.Account;
using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries;
using ERP.Domain.Models.Entities.Account.Attachments;
using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;

namespace ERP.Infrastracture.Services.Account.Entries;

public class EntryService : BaseService<Entry, EntryCreateCommand, EntryUpdateCommand>, IEntryService
{
    private IEntryRepository _repository;
    private IUnitOfWork _unitOfWork;
    public EntryService(IEntryRepository repository, IUnitOfWork unitOfWork) : base(repository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
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
                    Errors = bussinessValidationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }
            var entry = command.Adapt<Entry>();
            var entryNumberResult = await GetEntryNumber(entry.EntryDate);
            if (entryNumberResult.IsSuccess == false || entryNumberResult.Result == null)
                throw new Exception("FaildToGenerateEntryNubmer");

            var entryNumber = entryNumberResult.Result;
            entry.EntryNumber = entryNumber.EntryNumber;
            entry.FinancialPeriodId = entryNumber.FinancialPeriodId;
            
            entry.EntryType = command.Type;
            entry.FinancialTransactions = command.FinancialTransactions;

            if (command.Attachments != null && command.Attachments.Any())
                entry.EntryAttachments = command.Attachments.ToAttachment().Select(e => CreateEntryAttachment(e)).ToList();

            entry = await _unitOfWork.EntryRepository.Add(entry);
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
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public override async Task<ApiResponse<Entry>> Update(EntryUpdateCommand command, bool isValidate = true)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var bussinessValidationResult = await ValidateUpdate(command);
            if (!bussinessValidationResult.isValid || bussinessValidationResult.entity is null)
            {
                return new ApiResponse<Entry>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = bussinessValidationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
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
            await _unitOfWork.EntryRepository.Update(entry);
            await _unitOfWork.CommitAsync();
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
            await _unitOfWork.RollbackAsync();
            Log.Error(ex.ToString());
            return new ApiResponse<Entry>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<EntryDto>>> Get(EntryType? entryType = null)
    {
        var entries = await _unitOfWork.EntryRepository.GetQuery().Include(e => e.FinancialTransactions).Include(e => e.FinancialPeriod).Where(e => entryType == null ? true : e.EntryType == entryType)
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

        var financialPeriod = await _unitOfWork.FinancialPeriodRepository.Get(entry.FinancialPeriodId);

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
        IEnumerable<EntryAttachment> oldAttachments = await _unitOfWork.EntryAttachmentRepository.GetQuery()
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
            await _unitOfWork.EntryAttachmentRepository.Delete(deletedEntryAttachments);
            await _unitOfWork.AttachmentRepository.Delete(deletedAttachments);
        }

        var newEntryAttachments = newAttachments.Select(e =>
        {
            var entryAttachment = CreateEntryAttachment(e);
            entryAttachment.EntryId = entry.Id;
            return entryAttachment;
        });

        if (newAttachments.Any())
            await _unitOfWork.EntryAttachmentRepository.Add(newEntryAttachments);

        if (updatedAttachments.Any())
            await _unitOfWork.AttachmentRepository.Update(updatedAttachments);
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
                _unitOfWork.FinancialTransactionRepository.GetQuery().Where(e => e.EntryId.Equals(entry.Id)).ToList();

            IEnumerable<FinancialTransaction> deletedFinancialTransactions =
                existedFinancialTransactions.Where(f => !newFinancialTransactions.Any(e => e.Id == f.Id));

            List<FinancialTransaction> addedFinancialTransactions =
                newFinancialTransactions.Where(f => !existedFinancialTransactions.Any(e => e.Id == f.Id)).ToList();
            addedFinancialTransactions.ForEach(e => e.EntryId = entry.Id);
            List<FinancialTransaction> updatedFinancialTransactions = newFinancialTransactions
                .Where(f => existedFinancialTransactions.Any(e => e.Id == f.Id)).ToList();
            SyncCreationInfoToUpdatedTransactions(existedFinancialTransactions, updatedFinancialTransactions);

            if (addedFinancialTransactions.Any())
                await  _unitOfWork.FinancialTransactionRepository.Add(addedFinancialTransactions);
            if (updatedFinancialTransactions.Any())
                await  _unitOfWork.FinancialTransactionRepository.Update(updatedFinancialTransactions);
            if (deletedFinancialTransactions.Any())
                 await _unitOfWork.FinancialTransactionRepository.Delete(deletedFinancialTransactions);

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
                _unitOfWork.Set<EntryCostCenter>().Where(e => e.EntryId.Equals(entry.Id)).ToList();

            IEnumerable<EntryCostCenter> deletedCostCenters =
                existedCostCenters.Where(f => !newCostCenters.Any(e => e.Id == f.Id));

            List<EntryCostCenter> addedCostCenters =
                newCostCenters.Where(f => !existedCostCenters.Any(e => e.Id == f.Id)).ToList();
            addedCostCenters.ForEach(e => e.EntryId = entry.Id);
            List<EntryCostCenter> updatedFinancialTransactions = newCostCenters
                .Where(f => existedCostCenters.Any(e => e.Id == f.Id)).ToList();
            SyncCreationInfoToUpdatedCostCenters(existedCostCenters, updatedFinancialTransactions);

            if (addedCostCenters.Any())
                await _unitOfWork.Set<EntryCostCenter>().AddRangeAsync(addedCostCenters);
            if (updatedFinancialTransactions.Any())
                _unitOfWork.Set<EntryCostCenter>().UpdateRange(updatedFinancialTransactions);
            if (deletedCostCenters.Any())
                _unitOfWork.Set<EntryCostCenter>().RemoveRange(deletedCostCenters);

            await _unitOfWork.SaveChanges();
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

        var financialPeriod = await _unitOfWork.Set<FinancialPeriod>()
            .FirstOrDefaultAsync(e => e.StartDate <= dateTime && e.EndDate > dateTime);
        if (financialPeriod == null)
        {
            return new ApiResponse<EntryNumberDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.Found,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "NotFoundCurrentFinancialPeriod" } }
            };
        }

        result.FinancialPeriodId = financialPeriod.Id;
        result.FinancialPeriodNumber = financialPeriod.YearNumber ?? string.Empty;


    var lastEntryNumber = await _unitOfWork.Set<Entry>()
        .Where(e => e.FinancialPeriodId == financialPeriod.Id)
        .OrderByDescending(e => Convert.ToInt64(e.EntryNumber))
        .Select(e => Convert.ToInt64(e.EntryNumber))
        .FirstOrDefaultAsync();
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

        var financialPeriod = await _unitOfWork.Set<FinancialPeriod>()
            .FirstOrDefaultAsync(e => e.StartDate <= command.EntryDate && e.EndDate > command.EntryDate);
        if (financialPeriod == null || command.FinancialPeriodId != financialPeriod.Id)
        {
            validationResult.isValid = false;
            validationResult.errors.Add("NotFoundCurrentFinancialPeriod");
        }

        var entryWithSameNumber = await _unitOfWork.Set<Entry>()
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