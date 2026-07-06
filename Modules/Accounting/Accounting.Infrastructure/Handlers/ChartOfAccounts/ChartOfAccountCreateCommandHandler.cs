using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.CQRS.Handlers.TreeSetting;

namespace Accounting.Infrastructure.Handlers.ChartOfAccounts;

public sealed class ChartOfAccountCreateCommandHandler(IChartOfAccountRepository repository)
    : BaseTreeSettingCreateHandler<ChartOfAccount, ChartOfAccountCreateCommand>(repository)
{
    private IChartOfAccountRepository ChartOfAccountRepository => (IChartOfAccountRepository)SettingRepository;

    protected override async Task<CrudValidationResult> ValidateCreateAsync(
        ChartOfAccountCreateCommand command,
        CancellationToken cancellationToken)
    {
        var baseResult = await base.ValidateCreateAsync(command, cancellationToken);
        var errors = baseResult.Errors.ToList();

        var chartOfAccount = await ChartOfAccountRepository.GetChartOfAccountByCode(command.Code ?? "");
        if (chartOfAccount is not null)
            errors.Add(ErrorMessages.ChartOfAccountCodeExists);

        return errors.Count == 0
            ? CrudValidationResult.Success
            : CrudValidationResult.Failure(errors);
    }
}
