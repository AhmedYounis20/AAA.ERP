using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Utility;
using Microsoft.AspNetCore.Http;
using Shared.BaseEntities;
using Shared.Responses;

namespace AAA.ERP.Services.Impelementation.SubLeadgers;

public class CashInBoxService : ICashInBoxService
{
    private IUnitOfWork _unitOfWork;
    private IHttpContextAccessor _accessor;

    public CashInBoxService(IUnitOfWork unitOfWork, IHttpContextAccessor accessor)
    {
        _unitOfWork = unitOfWork;
        _accessor = accessor;
    }

    public async Task<ApiResponse<CashInBox>> Get(Guid id)
    {
        var temp = _accessor.HttpContext.User.Claims;
        var subLeadger = await _unitOfWork.CashInBoxRepository.Get(id);
        return new ApiResponse<CashInBox>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = subLeadger
        };
    }

    public async Task<ApiResponse<IEnumerable<CashInBox>>> Get()
    {
        IEnumerable<CashInBox> subLeadgers = await _unitOfWork.CashInBoxRepository.Get();

        return new ApiResponse<IEnumerable<CashInBox>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = subLeadgers
        };
    }

    public async Task<ApiResponse<BaseSubLeadgerInputModel>> GetNextSubLeadgers(Guid? parentId)
    {
        string? code =
            await _unitOfWork.ChartOfAccountRepository.GenerateNewCodeForChild(Guid.Parse(SD.ChartAccountId));
        BaseSubLeadgerInputModel inputModel = new BaseSubLeadgerInputModel
        {
            Code = code,
            NodeType = NodeType.Domain,
            ParentId = parentId
        };
        return new ApiResponse<BaseSubLeadgerInputModel>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = inputModel
        };
    }

    public async Task<ApiResponse<CashInBox>> Create(BaseSubLeadgerInputModel inputModel)
    {
        var temp = _accessor.HttpContext.User.Claims;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            bool isDomain = inputModel.NodeType.Equals(NodeType.Domain);
            ChartOfAccount? chartOfAccountParent =
                await _unitOfWork.ChartOfAccountRepository.Get(Guid.Parse(SD.ChartAccountId));
            string newCode = await _unitOfWork
                .ChartOfAccountRepository
                .GenerateNewCodeForChild(Guid.Parse(SD.ChartAccountId));
            CashInBox entity = new CashInBox
            {
                Name = inputModel.Name,
                NameSecondLanguage = inputModel.NameSecondLanguage,
                NodeType = inputModel.NodeType,
                ParentId = inputModel.ParentId,
                Notes = inputModel.Notes,
                ChartOfAccount = isDomain
                    ? new ChartOfAccount
                    {
                        Name = inputModel.Name,
                        NameSecondLanguage = inputModel.NameSecondLanguage,
                        ParentId = Guid.Parse(SD.ChartAccountId),
                        Code = newCode,
                        AccountNature = chartOfAccountParent?.AccountNature ?? AccountNature.Debit,
                        IsDepreciable = chartOfAccountParent?.IsDepreciable ?? false,
                        IsActiveAccount = chartOfAccountParent?.IsActiveAccount ?? false,
                        IsStopDealing = chartOfAccountParent?.IsStopDealing ?? true,
                        IsPostedAccount = chartOfAccountParent?.IsPostedAccount ?? false,
                        AccountGuidId = chartOfAccountParent?.AccountGuidId ?? Guid.NewGuid()
                    }
                    : null
            };

            await _unitOfWork.CashInBoxRepository.Add(entity);

            await _unitOfWork.CommitAsync();

            return new ApiResponse<CashInBox>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity,
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return new ApiResponse<CashInBox>
            {
                IsSuccess = false,
                ErrorMessages = [ex.ToString()]
            };
        }
    }

    public Task<ApiResponse<CashInBox>> Update(BaseSubLeadgerInputModel inputModel)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<CashInBox>> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}