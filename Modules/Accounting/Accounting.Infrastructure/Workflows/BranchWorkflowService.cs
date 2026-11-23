using Accounting.Application.Commands.Branches;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Attachments;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Domain.Entities;

namespace Accounting.Infrastructure.Workflows;

/// <summary>
/// Branch write orchestration (sub-ledger COA, attachments, transactional UoW).
/// </summary>
public class BranchWorkflowService(
    IUnitOfWork unitOfWork,
    IBranchRepository repository,
    IChartOfAccountRepository chartOfAccountRepository,
    IAttachmentStore attachmentStore,
    ISubLedgerCoaParentResolver subLedgerCoaParentResolver,
    ISubLedgerCoaFactory subLedgerCoaFactory) : IBranchWorkflowService
{
    public async Task<ApiResponse<Branch>> Create(BranchCreateCommand command)
    {
        var duplicateValidation = await ValidateDuplicateNames(command.Name, command.NameSecondLanguage);
        if (!duplicateValidation.isValid)
        {
            return new ApiResponse<Branch>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = duplicateValidation.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
            };
        }

        await unitOfWork.BeginTransactionAsync();
        try
        {
            var isDomain = command.NodeType.Equals(NodeType.Domain);
            Branch entity = new();

            if (isDomain)
            {
                entity = command.Adapt<Branch>();
                var chartOfAccountParent =
                    await subLedgerCoaParentResolver.ResolveAsync(SubLeadgerType.Branch);
                entity.ChartOfAccount = await subLedgerCoaFactory.CreateChildAccountAsync(
                    command.Name,
                    command.NameSecondLanguage,
                    chartOfAccountParent,
                    SubLeadgerType.Branch);
                if (command.Logo is { HasFileContent: true })
                {
                    entity.AttachmentId = await attachmentStore.SaveAsync(
                        command.Logo.FileName,
                        command.Logo.ContentType,
                        command.Logo.ToArray());
                }
            }
            else
            {
                entity.Name = command.Name;
                entity.NameSecondLanguage = command.NameSecondLanguage;
                entity.ParentId = command.ParentId;
                entity.NodeType = command.NodeType;
            }

            await repository.Add(entity);
            await unitOfWork.CommitAsync();
            return new ApiResponse<Branch>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            await unitOfWork.RollbackAsync();
            return new ApiResponse<Branch>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<Branch>> Update(BranchUpdateCommand command)
    {
        var validationResult = await ValidateUpdate(command);
        if (!validationResult.isValid)
        {
            return new ApiResponse<Branch>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
            };
        }

        if (command.NodeType == NodeType.Category)
            return await UpdateCategoryBranch(command);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            var entity = validationResult.entity!;
            entity.Name = command.Name;
            entity.NameSecondLanguage = command.NameSecondLanguage;
            entity.Phone = command.Phone;
            entity.Address = command.Address;
            entity.Notes = command.Notes;
            entity.ModifiedAt = DateTime.Now;
            entity.ChartOfAccount = null;
            entity.Attachment = null;
            if (entity.ChartOfAccountId.HasValue)
            {
                var account = await chartOfAccountRepository.Get(entity.ChartOfAccountId.Value);
                account.Name = command.Name;
                account.NameSecondLanguage = command.NameSecondLanguage;
                await chartOfAccountRepository.Update(account);
            }

            if (command.Logo is null && entity.AttachmentId.HasValue)
            {
                var removedId = entity.AttachmentId.Value;
                entity.AttachmentId = null;
                await attachmentStore.DeleteAsync([removedId]);
            }
            else if (command.Logo is { HasFileContent: true })
            {
                if (entity.AttachmentId.HasValue)
                {
                    await attachmentStore.ReplaceAsync(
                        entity.AttachmentId.Value,
                        command.Logo.FileName,
                        command.Logo.ContentType,
                        command.Logo.ToArray());
                }
                else
                {
                    entity.AttachmentId = await attachmentStore.SaveAsync(
                        command.Logo.FileName,
                        command.Logo.ContentType,
                        command.Logo.ToArray());
                }
            }

            entity = await repository.Update(entity);
            await unitOfWork.CommitAsync();
            return new ApiResponse<Branch>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            Log.Error(ex.ToString());
            return new ApiResponse<Branch>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ex.Message }],
                StatusCode = HttpStatusCode.BadRequest
            };
        }
    }

    public async Task<ApiResponse<Branch>> Delete(Guid id)
    {
        var validationResult = await ValidateDelete(id);
        if (!validationResult.isValid)
        {
            return new ApiResponse<Branch>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
            };
        }

        await unitOfWork.BeginTransactionAsync();
        try
        {
            var entity = validationResult.entity;
            if (entity is null)
            {
                await unitOfWork.RollbackAsync();
                return new ApiResponse<Branch>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            entity.ChartOfAccount = null;
            entity.Attachment = null;
            await repository.Delete(entity);
            if (entity.NodeType.Equals(NodeType.Domain))
            {
                if (entity.ChartOfAccountId.HasValue)
                    await chartOfAccountRepository.Delete(entity.ChartOfAccountId.Value);
                if (entity.AttachmentId.HasValue)
                    await attachmentStore.DeleteAsync([entity.AttachmentId.Value]);
            }

            await unitOfWork.CommitAsync();
            return new ApiResponse<Branch>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            Log.Error(ex.ToString());
            return new ApiResponse<Branch>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    private async Task<ApiResponse<Branch>> UpdateCategoryBranch(BranchUpdateCommand command)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var entity = await repository.Get(command.Id);
            if (entity is not null)
            {
                var newEntity = command.Adapt<Branch>();
                if (entity.ChartOfAccount is not null)
                {
                    newEntity.ChartOfAccountId = entity.ChartOfAccountId;
                    newEntity.ChartOfAccount = entity.ChartOfAccount;
                    newEntity.ChartOfAccount.Name = command.Name;
                    newEntity.ChartOfAccount.NameSecondLanguage = command.NameSecondLanguage;
                }

                await repository.Update(newEntity);
                await unitOfWork.CommitAsync();
                return new ApiResponse<Branch>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = newEntity
                };
            }

            await unitOfWork.RollbackAsync();
            return new ApiResponse<Branch>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.RecordNotFound }]
            };
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new ApiResponse<Branch>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    private async Task<(bool isValid, List<string> errors, Branch? entity)> ValidateUpdate(BranchUpdateCommand command)
    {
        var errors = new List<string>();
        var isValid = true;

        var oldEntity = await repository.Get(command.Id);
        var duplicateValidation = await ValidateDuplicateNames(command.Name, command.NameSecondLanguage, command.Id);
        if (!duplicateValidation.isValid)
        {
            isValid = false;
            errors.AddRange(duplicateValidation.errors!);
        }

        if (oldEntity is not null && oldEntity.NodeType != command.NodeType)
        {
            isValid = false;
            errors.Add(ErrorMessages.CannotChangeNodeType);
        }

        return (isValid, errors, oldEntity);
    }

    private async Task<(bool isValid, List<string> errors, Branch? entity)> ValidateDelete(Guid id)
    {
        if (await repository.HasChildren(id))
            return (false, ["CannotDeleteParent"], null);

        var entity = await repository.Get(id);
        if (entity is null)
            return (false, [ErrorMessages.RecordNotFound], null);

        return (true, [], entity);
    }

    private async Task<(bool isValid, List<string>? errors)> ValidateDuplicateNames(
        string? name,
        string? nameSecondLanguage,
        Guid? excludeId = null)
    {
        var existedEntity = await repository.GetByNames(name, nameSecondLanguage);
        if (existedEntity is null || (excludeId.HasValue && existedEntity.Id == excludeId.Value))
            return (true, null);

        var errors = new List<string>();
        if (existedEntity.Name?.Trim().ToUpper() == name?.Trim().ToUpper())
            errors.Add(ErrorMessages.WithSameNameIsExisted);
        if (existedEntity.NameSecondLanguage?.Trim().ToUpper() == nameSecondLanguage?.Trim().ToUpper())
            errors.Add(ErrorMessages.WithSameNameSecondLanguageIsExisted);

        return errors.Count == 0 ? (true, null) : (false, errors);
    }
}
