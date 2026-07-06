using Accounting.Application.Commands.Currencies;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.Currencies;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Infrastructure.Handlers.Currencies;

public sealed class CurrencyUpdateCommandHandler(ICurrencyRepository repository)
    : BaseSettingUpdateHandler<Currency, CurrencyUpdateCommand>(repository)
{
    private ICurrencyRepository CurrencyRepository => (ICurrencyRepository)SettingRepository;

    protected override async Task<CrudEntityValidationResult<Currency>> ValidateUpdateAsync(
        CurrencyUpdateCommand command,
        CancellationToken cancellationToken)
    {
        var baseResult = await base.ValidateUpdateAsync(command, cancellationToken);
        var errors = baseResult.Errors.ToList();

        var currencyWithSameSymbol = await CurrencyRepository.GetQuery()
            .Where(e => e.Symbol == command.Symbol)
            .FirstOrDefaultAsync(cancellationToken);

        if (currencyWithSameSymbol is not null && currencyWithSameSymbol.Id != command.Id)
            errors.Add(ErrorMessages.CurrencySymbolIsExisted);

        if (command.IsDefault)
        {
            var defaultCurrency = await CurrencyRepository.GetDefaultCurrency();
            if (defaultCurrency is not null && defaultCurrency.Id != command.Id)
                errors.Add(ErrorMessages.DefaultCurrencyIsAlreadyExistedWithName);
        }

        return errors.Count == 0
            ? CrudEntityValidationResult<Currency>.Success(baseResult.Entity!)
            : CrudEntityValidationResult<Currency>.Failure(errors, baseResult.Entity);
    }
}
