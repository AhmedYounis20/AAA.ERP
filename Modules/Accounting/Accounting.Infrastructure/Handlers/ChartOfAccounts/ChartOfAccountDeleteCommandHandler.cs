using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.CQRS.Handlers.TreeSetting;

namespace Accounting.Infrastructure.Handlers.ChartOfAccounts;

public sealed class ChartOfAccountDeleteCommandHandler(IChartOfAccountRepository repository)
    : BaseTreeSettingDeleteHandler<ChartOfAccount, ChartOfAccountDeleteCommand>(repository)
{
    protected override async Task<CrudEntityValidationResult<ChartOfAccount>> ValidateDeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var baseResult = await base.ValidateDeleteAsync(id, cancellationToken);
        if (!baseResult.IsValid || baseResult.Entity is null)
            return baseResult;

        var errors = new List<string>();

        if (baseResult.Entity.IsCreatedFromSubLeadger)
            errors.Add(ErrorMessages.ChartOfAccountFromSubLeadger);

        if (baseResult.Entity.IsSubLeadgerBaseAccount)
            errors.Add(ErrorMessages.ChartOfAccountIsSubLeadgerBase);

        return errors.Count == 0
            ? baseResult
            : CrudEntityValidationResult<ChartOfAccount>.Failure(errors, baseResult.Entity);
    }
}
