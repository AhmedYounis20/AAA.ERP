using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Validators.BussinessValidator.BaseBussinessValidators.Impelementation;
using Domain.Account.Validators.BussinessValidator.Interfaces;
using Microsoft.Extensions.Localization;
using Shared.Resources;

namespace Domain.Account.Validators.BussinessValidator.Impelementation;

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