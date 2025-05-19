using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.Attachments;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Repositories.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Infrastracture.Services.Account.SubLeadgers.SubLeadgerBaseService;

namespace ERP.Infrastracture.Services.Account.SubLeadgers;

public class BranchService : SubLeadgerService<Branch, BranchCreateCommand, BranchUpdateCommand>,
    IBranchService
{
    private IUnitOfWork _unitOfWork;
    private IHttpContextAccessor _accessor;
    private IBranchRepository _repository;

    public BranchService(IUnitOfWork unitOfWork, IHttpContextAccessor accessor)
        : base(unitOfWork, unitOfWork.BranchRepository, accessor, SD.BranchChartOfAccountId, SubLeadgerType.Branch)
    {
        _unitOfWork = unitOfWork;
        _accessor = accessor;
        _repository = unitOfWork.BranchRepository;
    }


    public override async Task<ApiResponse<Branch>> Create(BranchCreateCommand command, bool isValidate = true)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            bool isDomain = command.NodeType.Equals(NodeType.Domain);
            ChartOfAccount? chartOfAccountParent =
                await _unitOfWork.ChartOfAccountRepository.Get(Guid.Parse(SD.BranchChartOfAccountId));
            string newCode = await _unitOfWork
                .ChartOfAccountRepository
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
                    Attachment attachment = new();
                    attachment = new Attachment
                    {
                        FileData = command.Logo.ToArray(),
                        FileContentType = command.Logo.ContentType,
                        FileName = command.Logo.FileName
                    };
                    attachment = await _unitOfWork.AttachmentRepository.Add(attachment);
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

            await _unitOfWork.BranchRepository.Add(entity);
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
                ErrorMessages = [ex.Message]
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
                ErrorMessages = validationResult.errors
            };
        }

        if (command.NodeType == NodeType.Category)
            return await base.Update(command, false);
        else
        {
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
                    var account = await _unitOfWork.ChartOfAccountRepository.Get(entity.ChartOfAccountId.Value);
                    account.Name = command.Name;
                    account.NameSecondLanguage = command.NameSecondLanguage;
                    await _unitOfWork.ChartOfAccountRepository.Update(account);
                }

                if ((command.Logo == null || command.Logo.Length <= 0) && entity.AttachmentId.HasValue)
                {
                    await _unitOfWork.AttachmentRepository.Delete(entity.AttachmentId.Value);
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
                    attachment = await _unitOfWork.AttachmentRepository.Add(attachment);
                    entity.AttachmentId = attachment.Id;
                }
                else if (entity.AttachmentId.HasValue && command.Logo != null && command.Logo.Length > 0)
                {
                    Attachment attachment = await _unitOfWork.AttachmentRepository.Get(entity.AttachmentId.Value);
                    attachment.FileData = command.Logo.ToArray();
                    attachment.FileName = command.Logo.FileName;
                    attachment.FileContentType = command.Logo.ContentType;
                    attachment = await _unitOfWork.AttachmentRepository.Update(attachment);
                }

                entity = await _unitOfWork.BranchRepository.Update(entity);
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
                    ErrorMessages = [ex.ToString()],
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
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
                ErrorMessages = validationResult.errors
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
                    ErrorMessages = validationResult.errors
                };
            }

            entity.ChartOfAccount = null;
            entity.Attachment = null;
            await _repository.Delete(entity);
            if (entity.NodeType.Equals(NodeType.Domain))
            {
                if (entity.ChartOfAccountId.HasValue)
                    await _unitOfWork.ChartOfAccountRepository.Delete(entity.ChartOfAccountId.Value);
                if (entity.AttachmentId.HasValue)
                    await _unitOfWork.AttachmentRepository.Delete(entity.AttachmentId.Value);
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
                ErrorMessages = [ex.ToString()]
            };
        }
    }
}