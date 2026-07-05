using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.Infrastructure.Services.BaseServices;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Infrastructure.Services;

public class ChartOfAccountService :
    BaseTreeSettingService<ChartOfAccount, ChartOfAccountCreateCommand, ChartOfAccountUpdateCommand>,
    IChartOfAccountService
{
    private readonly IChartOfAccountRepository _repository;

    public ChartOfAccountService(IChartOfAccountRepository repository) : base(repository)
        => _repository = repository;

    public Task<string> GenerateNewCodeForChild(Guid? parentId)
        => _repository.GenerateNewCodeForChild(parentId);

    public async Task<ApiResponse<ChartOfAccountInputModel>> NextAccountDefaultData(Guid? parentId)
    {
        try
        {
            ChartOfAccount? parent = parentId is not null ? await _repository.Get(parentId ?? Guid.Empty) : null;
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
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new ApiResponse<ChartOfAccountInputModel>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                IsSuccess = false,
            };
        }
    }

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(ChartOfAccountCreateCommand command)
    {
        var result = await base.ValidateCreate(command);
        ChartOfAccount? chartOfAccount = await _repository.GetChartOfAccountByCode(command.Code ?? "");

        if (chartOfAccount != null)
        {
            result.isValid = false;
            result.errors.Add(ErrorMessages.ChartOfAccountCodeExists);
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
                result.errors.Add(ErrorMessages.ChartOfAccountCodeCannotChange);
            }
            if (chartOfAccount.IsCreatedFromSubLeadger && command.Name != chartOfAccount.Name)
            {
                result.isValid = false;
                result.errors.Add(ErrorMessages.ChartOfAccountUpdateSubLeadgerName);
            }
            else if (chartOfAccount.IsCreatedFromSubLeadger && command.NameSecondLanguage != chartOfAccount.NameSecondLanguage)
            {
                result.isValid = false;
                result.errors.Add(ErrorMessages.ChartOfAccountUpdateSubLeadgerNameSecondLanguage);
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
                result.errors.Add(ErrorMessages.ChartOfAccountFromSubLeadger);
            }
            if (result.entity.IsSubLeadgerBaseAccount)
            {
                result.isValid = false;
                result.errors.Add(ErrorMessages.ChartOfAccountIsSubLeadgerBase);
            }
        }

        return result;
    }
}
