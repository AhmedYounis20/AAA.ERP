using AAA.ERP.Services.BaseServices.impelemtation;
using AAA.ERP.Models.Entities.ChartOfAccount;
using AAA.ERP.Repositories.Interfaces;
using AAA.ERP.Services.Interfaces;
using AAA.ERP.Validators.BussinessValidator.Interfaces;
using AAA.ERP.Responses;
using AAA.ERP.InputModels;
using AAA.ERP.Models.Entities.FinancialPeriods;

namespace AAA.ERP.Services.Impelementation;
public class ChartOfAccountService : BaseTreeSettingService<ChartOfAccount>, IChartOfAccountService
{
    IChartOfAccountRepository _repo;
    public ChartOfAccountService(IChartOfAccountRepository repository, IChartOfAccountBussinessValidator bussinessValidator) : base(repository, bussinessValidator)
    => _repo = repository;
    public async Task<string> GenerateNewCodeForChild(Guid? parentId)
        => await _repo.GenerateNewCodeForChild(parentId);

    public override async Task<ApiResponse> Create(ChartOfAccount entity, bool isValidate = true)
    {
        entity.Code = await GenerateNewCodeForChild(entity.ParentId);
        entity.IsDepreciable = false;
        return await base.Create(entity, isValidate);
    }
    public async Task<ApiResponse> NextAccountDefaultData(Guid? parentId)
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

        return new ApiResponse
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true,
            Result = inputModel
        };
    }
}