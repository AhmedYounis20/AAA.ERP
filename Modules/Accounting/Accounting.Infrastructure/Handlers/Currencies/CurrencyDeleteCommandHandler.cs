using Accounting.Application.Commands.Currencies;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.Currencies;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

namespace Accounting.Infrastructure.Handlers.Currencies;

public sealed class CurrencyDeleteCommandHandler(ICurrencyRepository repository)
    : BaseSettingDeleteHandler<Currency, CurrencyDeleteCommand>(repository)
{
    protected override async Task<CrudEntityValidationResult<Currency>> ValidateDeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var baseResult = await base.ValidateDeleteAsync(id, cancellationToken);
        if (!baseResult.IsValid)
            return baseResult;

        if (baseResult.Entity!.IsDefault)
        {
            return CrudEntityValidationResult<Currency>.Failure(
                new[] { ErrorMessages.DefaultCurrencyCannotBeDeleted },
                baseResult.Entity);
        }

        if (await repository.IsUsedInEntries(id))
        {
            return CrudEntityValidationResult<Currency>.Failure(
                new[] { ErrorMessages.CurrencyIsUsedInEntries },
                baseResult.Entity);
        }

        return baseResult;
    }
}
