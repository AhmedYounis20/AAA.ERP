using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.CQRS.Handlers.TreeSetting;

namespace Accounting.Infrastructure.Handlers.ChartOfAccounts;

public sealed class ChartOfAccountUpdateCommandHandler(
    IChartOfAccountRepository repository,
    IAccountGuideRepository accountGuideRepository)
    : BaseTreeSettingUpdateHandler<ChartOfAccount, ChartOfAccountUpdateCommand>(repository)
{
    private IChartOfAccountRepository ChartOfAccountRepository => (IChartOfAccountRepository)SettingRepository;

    protected override async Task<CrudEntityValidationResult<ChartOfAccount>> ValidateUpdateAsync(
        ChartOfAccountUpdateCommand command,
        CancellationToken cancellationToken)
    {
        var baseResult = await base.ValidateUpdateAsync(command, cancellationToken);
        if (!baseResult.IsValid && baseResult.Entity is null)
            return baseResult;

        var errors = baseResult.Errors.ToList();

        if (!await accountGuideRepository.CheckIfInDatabase(command.AccountGuidId))
            errors.Add(ErrorMessages.AccountGuideNotFound);

        if (baseResult.Entity is not null)
        {
            if (command.Code != baseResult.Entity.Code)
                errors.Add(ErrorMessages.ChartOfAccountCodeCannotChange);

            if (baseResult.Entity.IsCreatedFromSubLeadger && command.Name != baseResult.Entity.Name)
                errors.Add(ErrorMessages.ChartOfAccountUpdateSubLeadgerName);

            if (baseResult.Entity.IsCreatedFromSubLeadger
                && command.NameSecondLanguage != baseResult.Entity.NameSecondLanguage)
                errors.Add(ErrorMessages.ChartOfAccountUpdateSubLeadgerNameSecondLanguage);

            if (await ChartOfAccountRepository.IsInUse(command.Id))
            {
                if (command.AccountNature != baseResult.Entity.AccountNature)
                    errors.Add(ErrorMessages.ChartOfAccountNatureCannotChange);

                if (command.AccountGuidId != baseResult.Entity.AccountGuidId)
                    errors.Add(ErrorMessages.ChartOfAccountAccountGuideCannotChange);
            }
        }

        return errors.Count == 0
            ? CrudEntityValidationResult<ChartOfAccount>.Success(baseResult.Entity!)
            : CrudEntityValidationResult<ChartOfAccount>.Failure(errors, baseResult.Entity);
    }
}
