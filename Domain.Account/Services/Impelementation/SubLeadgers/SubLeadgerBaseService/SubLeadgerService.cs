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

    public SubLeadgerService(IUnitOfWork unitOfWork, IBaseSubLeadgerRepository<TEntity> repository,
        IHttpContextAccessor accessor) : base(repository)
    {
        _unitOfWork = unitOfWork;
        _accessor = accessor;
        _repository = repository;
    }

    public async Task<ApiResponse<TCreateCommand>> GetNextSubLeadgers(Guid? parentId)
    {
        string? code =
            await _unitOfWork.ChartOfAccountRepository.GenerateNewCodeForChild(Guid.Parse(SD.ChartAccountId));
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
                await _unitOfWork.ChartOfAccountRepository.Get(Guid.Parse(SD.ChartAccountId));
            string newCode = await _unitOfWork
                .ChartOfAccountRepository
                .GenerateNewCodeForChild(Guid.Parse(SD.ChartAccountId));
            TEntity entity = command.Adapt<TEntity>();
            if (isDomain)
            {
                entity.ChartOfAccount = new ChartOfAccount
                {
                    Name = command.Name,
                    NameSecondLanguage = command.NameSecondLanguage,
                    ParentId = Guid.Parse(SD.ChartAccountId),
                    Code = newCode,
                    AccountNature = chartOfAccountParent?.AccountNature ?? AccountNature.Debit,
                    IsDepreciable = chartOfAccountParent?.IsDepreciable ?? false,
                    IsActiveAccount = chartOfAccountParent?.IsActiveAccount ?? false,
                    IsStopDealing = chartOfAccountParent?.IsStopDealing ?? true,
                    IsPostedAccount = chartOfAccountParent?.IsPostedAccount ?? false,
                    AccountGuidId = chartOfAccountParent?.AccountGuidId ?? Guid.NewGuid()
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
                    newEntity.ChartOfAccount = entity.ChartOfAccount;
                    newEntity.ChartOfAccount.Name = command.Name;
                    newEntity.ChartOfAccount.NameSecondLanguage = command.NameSecondLanguage;
                }

                await _repository.Update(entity);
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

    public Task<ApiResponse<CashInBox>> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}