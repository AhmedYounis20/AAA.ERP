using AAA.ERP.Models.Entities.ChartOfAccount;
using AAA.ERP.Repositories.Interfaces;
using AAA.ERP.Resources;
using AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Impelementation;
using AAA.ERP.Validators.BussinessValidator.Interfaces;
using Microsoft.Extensions.Localization;

namespace AAA.ERP.Validators.BussinessValidator.Impelementation;

public class ChartOfAccountBussinessValidator : BaseTreeSettingBussinessValidator<ChartOfAccount>, IChartOfAccountBussinessValidator
{
    IChartOfAccountRepository _repo;
    public ChartOfAccountBussinessValidator(IChartOfAccountRepository repository, IStringLocalizer<Resource> localizer) : base(repository, localizer)
    => _repo = repository;

    public override async Task<(bool IsValid, List<string> ListOfErrors, ChartOfAccount? entity)> ValidateCreateBussiness(ChartOfAccount inpuModel)
    {
        var result = await base.ValidateCreateBussiness(inpuModel);
        ChartOfAccount? chartOfAccount = await _repo.GetChartOfAccountByCode(inpuModel.Code ?? "");

        if (chartOfAccount != null) {
            result.IsValid = false;
            result.ListOfErrors.Add("ChartOfAccoutWithSameCodeExist");
        }

        return result;
    }
    public override async Task<(bool IsValid, List<string> ListOfErrors, ChartOfAccount? entity)> ValidateUpdateBussiness(ChartOfAccount inpuModel)
    {
        var result = await  base.ValidateUpdateBussiness(inpuModel);
        ChartOfAccount? chartOfAccount = await _repo.Get(inpuModel.Id);
        if (chartOfAccount != null)
        {
            if(inpuModel.Code != chartOfAccount.Code)
            {
                result.IsValid = false;
                result.ListOfErrors.Add("ChartOfAccountCodeCannotBeChanged");
            }
        }

        return result;  
    }
}