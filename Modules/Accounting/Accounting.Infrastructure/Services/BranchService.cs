using Accounting.Application.Commands.Branches;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Shared.Extensions;
using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Services.SubLeadgers;
using ERP.Shared.BaseEntities.Attachments;
using Serilog;
using Shared.BaseEntities;

namespace Accounting.Infrastructure.Services;

public class BranchService : SubLeadgerService<Branch, BranchCreateCommand, BranchUpdateCommand>, IBranchService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBranchRepository _repository;
    private readonly IAttachmentRepository _attachmentRepository;

    public BranchService(
        IUnitOfWork unitOfWork,
        IBranchRepository repository,
        IHttpContextAccessor accessor,
        IChartOfAccountRepository chartOfAccountRepository,
        IAttachmentRepository attachmentRepository)
        : base(unitOfWork, repository, accessor, chartOfAccountRepository, SD.BranchChartOfAccountId, SubLeadgerType.Branch)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _attachmentRepository = attachmentRepository;
    }

    public override async Task<ApiResponse<Branch>> Create(BranchCreateCommand command, bool isValidate = true)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            bool isDomain = command.NodeType.Equals(NodeType.Domain);
            ChartOfAccount? chartOfAccountParent =
                await _chartOfAccountRepository.Get(Guid.Parse(SD.BranchChartOfAccountId));
            string newCode = await _chartOfAccountRepository
                .GenerateNewCodeForChild(Guid.Parse(SD.BranchChartOfAccountId));
            Branch entity = new Branch();

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
                if (command.Logo != null)
                {
                    Attachment attachment = new()
                    {
                        FileData = command.Logo.ToArray(),
                        FileContentType = command.Logo.ContentType,
                        FileName = command.Logo.FileName
                    };
                    attachment = await _attachmentRepository.Add(attachment);
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

            await _repository.Add(entity);
            await _unitOfWork.CommitAsync();
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
            await _unitOfWork.RollbackAsync();
            return new ApiResponse<Branch>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public override async Task<ApiResponse<Branch>> Update(BranchUpdateCommand command, bool isValidate = true)
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
            return await base.Update(command, false);

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var entity = validationResult.entity;
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
                var account = await _chartOfAccountRepository.Get(entity.ChartOfAccountId.Value);
                account.Name = command.Name;
                account.NameSecondLanguage = command.NameSecondLanguage;
                await _chartOfAccountRepository.Update(account);
            }

            if ((command.Logo == null || command.Logo.Length <= 0) && entity.AttachmentId.HasValue)
            {
                await _attachmentRepository.Delete(entity.AttachmentId.Value);
                entity.AttachmentId = null;
            }
            else if (!entity.AttachmentId.HasValue && command.Logo != null && command.Logo.Length > 0)
            {
                Attachment attachment = new Attachment
                {
                    FileData = command.Logo.ToArray(),
                    FileContentType = command.Logo.ContentType,
                    FileName = command.Logo.FileName
                };
                attachment = await _attachmentRepository.Add(attachment);
                entity.AttachmentId = attachment.Id;
            }
            else if (entity.AttachmentId.HasValue && command.Logo != null && command.Logo.Length > 0)
            {
                Attachment attachment = await _attachmentRepository.Get(entity.AttachmentId.Value);
                attachment.FileData = command.Logo.ToArray();
                attachment.FileName = command.Logo.FileName;
                attachment.FileContentType = command.Logo.ContentType;
                attachment = await _attachmentRepository.Update(attachment);
            }

            entity = await _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new ApiResponse<Branch>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            Log.Error(ex.ToString());
            return new ApiResponse<Branch>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ex.ToString() }],
                StatusCode = HttpStatusCode.BadRequest
            };
        }
    }

    public override async Task<ApiResponse<Branch>> Delete(Guid id, bool isValidate = true)
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

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var entity = validationResult.entity;
            if (entity == null)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<Branch>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            entity.ChartOfAccount = null;
            entity.Attachment = null;
            await _repository.Delete(entity);
            if (entity.NodeType.Equals(NodeType.Domain))
            {
                if (entity.ChartOfAccountId.HasValue)
                    await _chartOfAccountRepository.Delete(entity.ChartOfAccountId.Value);
                if (entity.AttachmentId.HasValue)
                    await _attachmentRepository.Delete(entity.AttachmentId.Value);
            }

            await _unitOfWork.CommitAsync();
            return new ApiResponse<Branch>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            Log.Error(ex.ToString());
            return new ApiResponse<Branch>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.ToString() }]
            };
        }
    }
}
