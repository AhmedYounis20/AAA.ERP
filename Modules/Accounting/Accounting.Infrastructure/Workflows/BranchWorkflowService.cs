using Accounting.Application.Commands.Branches;
using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.Extensions;
using ERP.BuildingBlocks.Domain.Entities;
using ERP.BuildingBlocks.Domain.Entities.Attachments;

namespace Accounting.Infrastructure.Workflows;

/// <summary>
/// Branch write orchestration (sub-ledger COA, attachments, transactional UoW).
/// </summary>
public class BranchWorkflowService(
    IUnitOfWork unitOfWork,
    IBranchRepository repository,
    IChartOfAccountRepository chartOfAccountRepository,
    IAttachmentRepository attachmentRepository) : IBranchWorkflowService
{
    public async Task<ApiResponse<Branch>> Create(BranchCreateCommand command)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var isDomain = command.NodeType.Equals(NodeType.Domain);
            var chartOfAccountParent =
                await chartOfAccountRepository.Get(Guid.Parse(SD.BranchChartOfAccountId));
            var newCode = await chartOfAccountRepository
                .GenerateNewCodeForChild(Guid.Parse(SD.BranchChartOfAccountId));
            Branch entity = new();

            if (isDomain)
            {
                entity = command.Adapt<Branch>();
                entity.ChartOfAccount = new ChartOfAccount
                {
                    Name = command.Name,
                    NameSecondLanguage = command.NameSecondLanguage,
                    ParentId = Guid.Parse(SD.BranchChartOfAccountId),
                    Code = newCode,
                    AccountNature = chartOfAccountParent?.AccountNature ?? AccountNature.Debit,
                    IsDepreciable = chartOfAccountParent?.IsDepreciable ?? false,
                    IsActiveAccount = chartOfAccountParent?.IsActiveAccount ?? false,
                    IsStopDealing = chartOfAccountParent?.IsStopDealing ?? true,
                    IsPostedAccount = chartOfAccountParent?.IsPostedAccount ?? false,
                    AccountGuidId = chartOfAccountParent?.AccountGuidId ?? Guid.NewGuid(),
                    IsCreatedFromSubLeadger = true,
                    SubLeadgerType = SubLeadgerType.Branch
                };
                if (command.Logo is not null)
                {
                    var attachment = new Attachment
                    {
                        FileData = command.Logo.ToArray(),
                        FileContentType = command.Logo.ContentType,
                        FileName = command.Logo.FileName
                    };
                    attachment = await attachmentRepository.Add(attachment);
                    entity.AttachmentId = attachment.Id;
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

            if ((command.Logo is null || command.Logo.Length <= 0) && entity.AttachmentId.HasValue)
            {
                await attachmentRepository.Delete(entity.AttachmentId.Value);
                entity.AttachmentId = null;
            }
            else if (!entity.AttachmentId.HasValue && command.Logo is not null && command.Logo.Length > 0)
            {
                var attachment = new Attachment
                {
                    FileData = command.Logo.ToArray(),
                    FileContentType = command.Logo.ContentType,
                    FileName = command.Logo.FileName
                };
                attachment = await attachmentRepository.Add(attachment);
                entity.AttachmentId = attachment.Id;
            }
            else if (entity.AttachmentId.HasValue && command.Logo is not null && command.Logo.Length > 0)
            {
                var attachment = await attachmentRepository.Get(entity.AttachmentId.Value);
                attachment.FileData = command.Logo.ToArray();
                attachment.FileName = command.Logo.FileName;
                attachment.FileContentType = command.Logo.ContentType;
                attachment = await attachmentRepository.Update(attachment);
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
                Errors = [new MessageTemplate { MessageKey = ex.ToString() }],
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
                    await attachmentRepository.Delete(entity.AttachmentId.Value);
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
                Errors = [new MessageTemplate { MessageKey = ex.ToString() }]
            };
        }
    }

    private async Task<ApiResponse<Branch>> UpdateCategoryBranch(BranchUpdateCommand command)
    {
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
            }

            return new ApiResponse<Branch>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Branch>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ex.ToString() }]
            };
        }
    }

    private async Task<(bool isValid, List<string> errors, Branch? entity)> ValidateUpdate(BranchUpdateCommand command)
    {
        var errors = new List<string>();
        var isValid = true;

        var oldEntity = await repository.Get(command.Id);
        var existedEntity = await repository.GetByNames(command.Name, command.NameSecondLanguage);
        if (existedEntity is not null && existedEntity.Id != command.Id)
        {
            isValid = false;
            if (existedEntity.Name.Trim().ToUpper() == command.Name?.Trim().ToUpper())
                errors.Add("WithSameNameIsExisted");
            if (existedEntity.NameSecondLanguage.Trim().ToUpper() == command.NameSecondLanguage?.Trim().ToUpper())
                errors.Add("WithSameNameSecondLanguageIsExisted");
        }

        if (oldEntity is not null && oldEntity.NodeType != command.NodeType)
        {
            isValid = false;
            errors.Add(ErrorMessages.NodeTypeCannotBeChanged);
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
}
