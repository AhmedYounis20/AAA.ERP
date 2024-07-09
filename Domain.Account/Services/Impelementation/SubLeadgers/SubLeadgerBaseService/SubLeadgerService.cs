using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Repositories.Interfaces.SubLeadgers;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Utility;
using Mapster;
using Microsoft.AspNetCore.Http;
using Serilog;
using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.Services.Impelementation.SubLeadgers.SubLeadgerBaseService;

public class SubLeadgerService<TEntity, TCreateCommand, TUpdateCommand> :
    BaseTreeSettingService<TEntity, TCreateCommand, TUpdateCommand>,
    IBaseSubLeadgerService<TEntity, TCreateCommand, TUpdateCommand>
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TCreateCommand : BaseSubLeadgerCreateCommand<TEntity>
    where TUpdateCommand : BaseSubLeadgerUpdateCommand<TEntity>

{
    private IUnitOfWork _unitOfWork;
    private IBaseSubLeadgerRepository<TEntity> _repository;
    private IHttpContextAccessor _accessor;
    private string _accountId;

    public SubLeadgerService(IUnitOfWork unitOfWork, IBaseSubLeadgerRepository<TEntity> repository,
        IHttpContextAccessor accessor, string accountId) : base(repository)
    {
        _unitOfWork = unitOfWork;
        _accessor = accessor;
        _repository = repository;
        _accountId = accountId;
    }

    public virtual async Task<ApiResponse<TCreateCommand>> GetNextSubLeadgers(Guid? parentId)
    {
        string? code =
            await _unitOfWork.ChartOfAccountRepository.GenerateNewCodeForChild(Guid.Parse(_accountId));
        TCreateCommand inputModel = Activator.CreateInstance<TCreateCommand>();

        inputModel.Code = code;
        inputModel.NodeType = NodeType.Domain;
        inputModel.ParentId = parentId;

        return new ApiResponse<TCreateCommand>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = inputModel
        };
    }

    public override async Task<ApiResponse<TEntity>> Create(TCreateCommand command, bool isValidate = true)
    {
        try
        {
            bool isDomain = command.NodeType.Equals(NodeType.Domain);
            ChartOfAccount? chartOfAccountParent =
                await _unitOfWork.ChartOfAccountRepository.Get(Guid.Parse(_accountId));
            string newCode = await _unitOfWork
                .ChartOfAccountRepository
                .GenerateNewCodeForChild(Guid.Parse(_accountId));
            TEntity entity = command.Adapt<TEntity>();
            if (isDomain)
            {
                entity.ChartOfAccount = new ChartOfAccount
                {
                    Name = command.Name,
                    NameSecondLanguage = command.NameSecondLanguage,
                    ParentId = Guid.Parse(_accountId),
                    Code = newCode,
                    AccountNature = chartOfAccountParent?.AccountNature ?? AccountNature.Debit,
                    IsDepreciable = chartOfAccountParent?.IsDepreciable ?? false,
                    IsActiveAccount = chartOfAccountParent?.IsActiveAccount ?? false,
                    IsStopDealing = chartOfAccountParent?.IsStopDealing ?? true,
                    IsPostedAccount = chartOfAccountParent?.IsPostedAccount ?? false,
                    AccountGuidId = chartOfAccountParent?.AccountGuidId ?? Guid.NewGuid(),
                    IsCreatedFromSubLeadger = true
                };
            }
            else
            {
                entity.ChartOfAccountId = null;
                entity.ChartOfAccount = null;
            }
            await _repository.Add(entity);

            return new ApiResponse<TEntity>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<TEntity>
            {
                IsSuccess = false,
                ErrorMessages = [ex.ToString()]
            };
        }
    }

    public override async Task<ApiResponse<TEntity>> Update(TUpdateCommand command, bool isValidate = true)
    {
        try
        {
            bool isDomain = command.NodeType.Equals(NodeType.Domain);
            TEntity? entity = await _repository.Get(command.Id);
            if (entity != null)
            {
                var newEntity = command.Adapt<TEntity>();

                if (entity.ChartOfAccount is not null)
                {
                    newEntity.ChartOfAccountId = entity.ChartOfAccountId;
                    newEntity.ChartOfAccount = entity.ChartOfAccount;
                    newEntity.ChartOfAccount.Name = command.Name;
                    newEntity.ChartOfAccount.NameSecondLanguage = command.NameSecondLanguage;
                }

                await _repository.Update(newEntity);
            }

            return new ApiResponse<TEntity>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<TEntity>
            {
                IsSuccess = false,
                ErrorMessages = [ex.ToString()]
            };
        }
    }

    public override async Task<ApiResponse<TEntity>> Delete(Guid id, bool isValidate = true)
    {
        var validationResult = await ValidateDelete(id);
        if (validationResult.isValid)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (validationResult.entity is not null)
                {

                    var entity = validationResult.entity;
                    entity.ChartOfAccount = null;
                    await _repository.Delete(entity);

                    if (entity.ChartOfAccountId.HasValue)
                        await _unitOfWork.ChartOfAccountRepository.Delete(entity.ChartOfAccountId.Value);
                }
                await _unitOfWork.CommitAsync();

                return new ApiResponse<TEntity>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = validationResult.entity
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                Log.Error(ex.Message.ToString());
                return new ApiResponse<TEntity>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = [ex.Message.Trim().ToString()]
                };
            }
        }

        return new ApiResponse<TEntity>
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.BadRequest,
            ErrorMessages = validationResult.errors
        };
    }

    protected override async Task<(bool isValid, List<string> errors, TEntity? entity)> ValidateUpdate(TUpdateCommand command)
    {
        var result = await base.ValidateUpdate(command);
        if (result.isValid)
        {
            if (result.entity.NodeType != command.NodeType)
            {
                result.isValid = false;
                result.errors.Add("CannotChangeNodeType");
            }
        }

        return result;
    }
}