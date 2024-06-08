using Domain.Account.Commands.ChartOfAccounts;
using Domain.Account.InputModels;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces;
using Domain.Account.Validators.BussinessValidator.Interfaces;
using Shared.Responses;

namespace Domain.Account.Services.Impelementation;

public class ChartOfAccountService :
    BaseTreeSettingService<ChartOfAccount, ChartOfAccountCreateCommand, ChartOfAccountUpdateCommand>,
    IChartOfAccountService
{
    IChartOfAccountRepository _repository;

    public ChartOfAccountService(IChartOfAccountRepository repository) : base(repository)
        => _repository = repository;

    public async Task<string> GenerateNewCodeForChild(Guid? parentId)
        => await _repository.GenerateNewCodeForChild(parentId);

    public async Task<ApiResponse<ChartOfAccountInputModel>> NextAccountDefaultData(Guid? parentId)
    {
        ChartOfAccount? parent = parentId is not null ? (await _repository.Get(parentId ?? Guid.Empty)) : null;
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

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(ChartOfAccountCreateCommand command)
    {
        var result = await base.ValidateCreate(command);
        ChartOfAccount? chartOfAccount = await _repository.GetChartOfAccountByCode(command.Code ?? "");

        if (chartOfAccount != null)
        {
            result.isValid = false;
            result.errors.Add("ChartOfAccoutWithSameCodeExist");
        }

        return result;

    }

    protected override async Task<(bool isValid, List<string> errors, ChartOfAccount? entity)> ValidateUpdate(ChartOfAccountUpdateCommand command)
    {
        var result = await base.ValidateUpdate(command);
        ChartOfAccount? chartOfAccount = await _repository.Get(command.Id);
        if (chartOfAccount != null)
        {
            if (command.Code != chartOfAccount.Code)
            {
                result.isValid = false;
                result.errors.Add("ChartOfAccountCodeCannotBeChanged");
            }
            if (chartOfAccount.IsCreatedFromSubLeadger && command.Name != chartOfAccount.Name)
            {
                result.isValid = false;
                result.errors.Add("ChartOfAccountUpdateSubLeadgerName");
            }
            else if (chartOfAccount.IsCreatedFromSubLeadger && command.NameSecondLanguage != chartOfAccount.NameSecondLanguage)
            {
                result.isValid = false;
                result.errors.Add("ChartOfAccountUpdateSubLeadgerNameSecondLanguage");
            }
        }

        return result;
    }

    protected override async Task<(bool isValid, List<string> errors, ChartOfAccount? entity)> ValidateDelete(Guid id)
    {
        var result = await base.ValidateDelete(id);
        if (result.isValid && result.entity is not null)
        {
            if (result.entity.IsCreatedFromSubLeadger)
            {
                result.isValid = false;
                result.errors.Add("ChartOfAccountCreatedFomSubLeadger");
            }
            if (result.entity.IsSubLeadgerBaseAccount)
            {
                result.isValid = false;
                result.errors.Add("ChartOfAccountBaseSubLeadgerAccount");
            }
        }

        return result;
    }
}