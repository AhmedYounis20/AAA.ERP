using Accounting.Application.Commands.Entries;
using Accounting.Application.Repositories.Account;
using Accounting.Application.Services;
using Accounting.Contracts.Dtos;
using Accounting.Domain.Models.Entities.Entries;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using Accounting.Infrastructure.Services.Entries;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Domain.Entities.Attachments;

namespace Accounting.Infrastructure.Workflows;

/// <summary>
/// Core entry write orchestration (numbering, financial period, attachments, transactions).
/// </summary>
public class EntryWorkflowService(
    IEntryRepository repository,
    IUnitOfWork unitOfWork,
    IBaseRepository<FinancialTransaction> financialTransactionRepository,
    IBaseRepository<EntryAttachment> entryAttachmentRepository,
    IAttachmentRepository attachmentRepository) : IEntryWorkflowService
{
    public async Task<ApiResponse<Entry>> Create(EntryCreateCommand command)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var bussinessValidationResult = await ValidateCreate(command);
            if (!bussinessValidationResult.isValid)
            {
                await unitOfWork.RollbackAsync();
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
            {
                await unitOfWork.RollbackAsync();
                return new ApiResponse<Entry>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = entryNumberResult.Errors ??
                             [new MessageTemplate { MessageKey = ErrorMessages.FaildToGenerateEntryNubmer }]
                };
            }

            var entryNumber = entryNumberResult.Result;
            if (await EntryNumberSupport.EntryNumberExistsAsync(
                    unitOfWork.Set<Entry>(),
                    entryNumber.FinancialPeriodId,
                    entryNumber.EntryNumber))
            {
                await unitOfWork.RollbackAsync();
                return new ApiResponse<Entry>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = [new MessageTemplate { MessageKey = ErrorMessages.EntryNumberExists }]
                };
            }

            entry.EntryNumber = entryNumber.EntryNumber;
            entry.FinancialPeriodId = entryNumber.FinancialPeriodId;

            entry.EntryType = command.Type;
            entry.FinancialTransactions = command.FinancialTransactions;

            if (command.Attachments != null && command.Attachments.Any())
                entry.EntryAttachments = command.Attachments.ToAttachment().Select(CreateEntryAttachment).ToList();

            entry = await repository.Add(entry);
            entry.FinancialTransactions = [];

            await unitOfWork.CommitAsync();

            return new ApiResponse<Entry>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entry
            };
        }
        catch (DbUpdateException ex) when (IsDuplicateEntryNumberViolation(ex))
        {
            await unitOfWork.RollbackAsync();
            return new ApiResponse<Entry>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.EntryNumberExists }]
            };
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            Log.Error(ex.ToString());
            return new ApiResponse<Entry>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<Entry>> Update(EntryUpdateCommand command)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var bussinessValidationResult = await ValidateUpdate(command);
            if (!bussinessValidationResult.isValid || bussinessValidationResult.entity is null)
            {
                await unitOfWork.RollbackAsync();
                return new ApiResponse<Entry>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = bussinessValidationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            var entry = bussinessValidationResult.entity;

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
            await repository.Update(entry);
            await unitOfWork.CommitAsync();
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
            await unitOfWork.RollbackAsync();
            Log.Error(ex.ToString());
            return new ApiResponse<Entry>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<Entry>> Delete(Guid id)
    {
        try
        {
            var validationResult = await ValidateDelete(id);
            if (!validationResult.isValid)
            {
                return new ApiResponse<Entry>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            var entity = validationResult.entity;
            if (entity is not null)
                await repository.Delete(entity);

            return new ApiResponse<Entry>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Entry>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    private Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => EntryNumberSupport.GetNextEntryNumberAsync(
            unitOfWork.Set<FinancialPeriod>(),
            unitOfWork.Set<Entry>(),
            dateTime);

    private async Task UpdateEntryAttachments(EntryUpdateCommand command, Entry entry)
    {
        IEnumerable<EntryAttachment> oldAttachments = await entryAttachmentRepository.GetQuery()
            .Include(e => e.Attachment)
            .Where(e => e.EntryId == entry.Id).ToListAsync();

        IEnumerable<EntryAttachment> deletedEntryAttachments =
            oldAttachments.Where(e => !command.Attachments.Any(file => file.AttachmentId == e.Attachment.Id));
        IEnumerable<Attachment> deletedAttachments = deletedEntryAttachments.Select(e => e.Attachment);
        IEnumerable<Attachment> newAttachments = command.Attachments
            .Where(file => !oldAttachments.Any(e => file.AttachmentId == e.Attachment.Id))
            .ToAttachment();
        List<Attachment> updatedAttachments = oldAttachments
            .Where(e => command.Attachments.Any(file => file.AttachmentId == e.Attachment.Id))
            .Select(e => e.Attachment)
            .ToList();

        SyncUpdatedEntryAttachments(updatedAttachments, command.Attachments);
        if (deletedAttachments.Any())
        {
            await entryAttachmentRepository.Delete(deletedEntryAttachments);
            await attachmentRepository.Delete(deletedAttachments);
        }

        var newEntryAttachments = newAttachments.Select(e =>
        {
            var entryAttachment = CreateEntryAttachment(e);
            entryAttachment.EntryId = entry.Id;
            return entryAttachment;
        });

        if (newAttachments.Any())
            await entryAttachmentRepository.Add(newEntryAttachments);

        if (updatedAttachments.Any())
            await attachmentRepository.Update(updatedAttachments);
    }

    private static EntryAttachment CreateEntryAttachment(Attachment attachment)
        => new() { Attachment = attachment };

    private static void SyncUpdatedEntryAttachments(List<Attachment> oldAttachments,
        List<AttachmentDto> newAttachments)
    {
        foreach (var oldAttachment in oldAttachments)
        {
            var newAttachment = newAttachments.First(e => e.AttachmentId == oldAttachment.Id);
            newAttachment.CopyTo(oldAttachment);
        }
    }

    private async Task UpdateFinancialTransactions(Entry entry, IEnumerable<FinancialTransaction> newFinancialTransactions)
    {
        try
        {
            List<FinancialTransaction> existedFinancialTransactions =
                financialTransactionRepository.GetQuery().Where(e => e.EntryId.Equals(entry.Id)).ToList();

            IEnumerable<FinancialTransaction> deletedFinancialTransactions =
                existedFinancialTransactions.Where(f => !newFinancialTransactions.Any(e => e.Id == f.Id));

            List<FinancialTransaction> addedFinancialTransactions =
                newFinancialTransactions.Where(f => !existedFinancialTransactions.Any(e => e.Id == f.Id)).ToList();
            addedFinancialTransactions.ForEach(e => e.EntryId = entry.Id);
            List<FinancialTransaction> updatedFinancialTransactions = newFinancialTransactions
                .Where(f => existedFinancialTransactions.Any(e => e.Id == f.Id)).ToList();
            SyncCreationInfoToUpdatedTransactions(existedFinancialTransactions, updatedFinancialTransactions);

            if (addedFinancialTransactions.Any())
                await financialTransactionRepository.Add(addedFinancialTransactions);
            if (updatedFinancialTransactions.Any())
                await financialTransactionRepository.Update(updatedFinancialTransactions);
            if (deletedFinancialTransactions.Any())
                await financialTransactionRepository.Delete(deletedFinancialTransactions);
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            throw;
        }
    }

    private async Task UpdateEntryCostCenter(Entry entry, IEnumerable<EntryCostCenter> newCostCenters)
    {
        try
        {
            List<EntryCostCenter> existedCostCenters =
                unitOfWork.Set<EntryCostCenter>().Where(e => e.EntryId.Equals(entry.Id)).ToList();

            IEnumerable<EntryCostCenter> deletedCostCenters =
                existedCostCenters.Where(f => !newCostCenters.Any(e => e.Id == f.Id));

            List<EntryCostCenter> addedCostCenters =
                newCostCenters.Where(f => !existedCostCenters.Any(e => e.Id == f.Id)).ToList();
            addedCostCenters.ForEach(e => e.EntryId = entry.Id);
            List<EntryCostCenter> updatedFinancialTransactions = newCostCenters
                .Where(f => existedCostCenters.Any(e => e.Id == f.Id)).ToList();
            SyncCreationInfoToUpdatedCostCenters(existedCostCenters, updatedFinancialTransactions);

            if (addedCostCenters.Any())
                await unitOfWork.Set<EntryCostCenter>().AddRangeAsync(addedCostCenters);
            if (updatedFinancialTransactions.Any())
                unitOfWork.Set<EntryCostCenter>().UpdateRange(updatedFinancialTransactions);
            if (deletedCostCenters.Any())
                unitOfWork.Set<EntryCostCenter>().RemoveRange(deletedCostCenters);

            await unitOfWork.SaveChanges();
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            throw;
        }
    }

    private static void SyncCreationInfoToUpdatedTransactions(
        IEnumerable<FinancialTransaction> oldFinancialTransactions,
        List<FinancialTransaction> newFinancialTransactions)
    {
        foreach (var transaction in newFinancialTransactions)
        {
            var oldTransaction = oldFinancialTransactions.FirstOrDefault(e => e.Id == transaction.Id);
            if (oldTransaction != null)
            {
                transaction.CreatedAt = oldTransaction.CreatedAt;
                transaction.CreatedBy = oldTransaction.CreatedBy;
                transaction.EntryId = oldTransaction.EntryId;
            }
        }
    }

    private static void SyncCreationInfoToUpdatedCostCenters(
        IEnumerable<EntryCostCenter> oldCostCenters,
        List<EntryCostCenter> newCostCenters)
    {
        foreach (var costCenter in newCostCenters)
        {
            var oldCostCenter = oldCostCenters.FirstOrDefault(e => e.Id == costCenter.Id);
            if (oldCostCenter != null)
            {
                costCenter.CreatedAt = oldCostCenter.CreatedAt;
                costCenter.CreatedBy = oldCostCenter.CreatedBy;
                costCenter.EntryId = oldCostCenter.EntryId;
            }
        }
    }

    private async Task<(bool isValid, List<string> errors)> ValidateCreate(EntryCreateCommand command)
    {
        var validationResult = (isValid: true, errors: new List<string>());

        var financialPeriod = await unitOfWork.Set<FinancialPeriod>()
            .FirstOrDefaultAsync(e => e.StartDate <= command.EntryDate && e.EndDate > command.EntryDate);
        if (financialPeriod == null || command.FinancialPeriodId != financialPeriod.Id)
        {
            validationResult.isValid = false;
            validationResult.errors.Add(ErrorMessages.FinancialPeriodNotFound);
        }

        var entryWithSameNumber = await unitOfWork.Set<Entry>()
            .Where(e => e.EntryNumber == command.EntryNumber && e.FinancialPeriodId == command.FinancialPeriodId)
            .FirstOrDefaultAsync();
        if (entryWithSameNumber != null)
        {
            validationResult.isValid = false;
            validationResult.errors.Add(ErrorMessages.EntryNumberExists);
        }

        return validationResult;
    }

    private async Task<(bool isValid, List<string> errors, Entry? entity)> ValidateUpdate(EntryUpdateCommand command)
    {
        var entity = await repository.Get(command.Id);
        if (entity == null)
        {
            return (false, [ErrorMessages.RecordNotFound], null);
        }

        return (true, [], entity);
    }

    private async Task<(bool isValid, List<string> errors, Entry? entity)> ValidateDelete(Guid id)
    {
        var entity = await repository.Get(id);
        if (entity is null)
            return (false, [ErrorMessages.RecordNotFound], null);

        return (true, [], entity);
    }

    private static bool IsDuplicateEntryNumberViolation(DbUpdateException exception)
    {
        var message = exception.InnerException?.Message ?? exception.Message;
        return message.Contains("FinancialPeriodId", StringComparison.OrdinalIgnoreCase)
               && message.Contains("EntryNumber", StringComparison.OrdinalIgnoreCase)
               && message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase);
    }
}
