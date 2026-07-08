using Accounting.Application.Commands.AccountGuides;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.AccountGuides;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

namespace Accounting.Infrastructure.Handlers.AccountGuides;

public sealed class AccountGuideDeleteCommandHandler(IAccountGuideRepository repository)
    : BaseSettingDeleteHandler<AccountGuide, AccountGuideDeleteCommand>(repository)
{
    protected override async Task<CrudEntityValidationResult<AccountGuide>> ValidateDeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var baseResult = await base.ValidateDeleteAsync(id, cancellationToken);
        if (!baseResult.IsValid)
            return baseResult;

        if (await repository.IsUsedInChartOfAccounts(id))
        {
            return CrudEntityValidationResult<AccountGuide>.Failure(
                new[] { ErrorMessages.AccountGuideIsUsedInChartOfAccounts },
                baseResult.Entity);
        }

        return baseResult;
    }
}
