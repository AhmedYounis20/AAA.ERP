using Domain.Account.InputModels;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces;
using Domain.Account.Validators.BussinessValidator.Interfaces;
using Shared.Responses;

namespace Domain.Account.Services.Impelementation;
public class ChartOfAccountService : BaseTreeSettingService<ChartOfAccount>, IChartOfAccountService
{
    IChartOfAccountRepository _repo;
    public ChartOfAccountService(IChartOfAccountRepository repository, IChartOfAccountBussinessValidator bussinessValidator) : base(repository, bussinessValidator)
    => _repo = repository;
    public async Task<string> GenerateNewCodeForChild(Guid? parentId)
        => await _repo.GenerateNewCodeForChild(parentId);

    public override async Task<ApiResponse<ChartOfAccount>> Create(ChartOfAccount entity, bool isValidate = true)
    {
        entity.Code = await GenerateNewCodeForChild(entity.ParentId);
        entity.IsDepreciable = false;
        return await base.Create(entity, isValidate);
    }
    public async Task<ApiResponse<ChartOfAccountInputModel>> NextAccountDefaultData(Guid? parentId)
    {
        ChartOfAccount? parent = parentId is not null  ? (await _repo.Get(parentId?? Guid.Empty)): null;
        ChartOfAccountInputModel inputModel = new ChartOfAccountInputModel
        {
            parentId = parentId,
            AccountNature = parent?.AccountNature ?? AccountNature.Debit,
            AccountGuidId = parent?.AccountGuidId ?? Guid.Empty,
            Code = await GenerateNewCodeForChild(parentId),
            IsPostedAccount = parent?.IsPostedAccount ?? false,
            IsStopDealing = parent?.IsStopDealing ?? false,
            IsDepreciable = false,
            IsActiveAccount = true,
        };

        return new ApiResponse<ChartOfAccountInputModel>
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true,
            Result = inputModel
        };
    }
}