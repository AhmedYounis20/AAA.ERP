using Accounting.Application.Commands.Currencies;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.Currencies;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

namespace Accounting.Infrastructure.Handlers.Currencies;

public sealed class CurrencyCreateCommandHandler(ICurrencyRepository repository)
    : BaseSettingCreateHandler<Currency, CurrencyCreateCommand>(repository)
{
    private ICurrencyRepository CurrencyRepository => (ICurrencyRepository)SettingRepository;

    protected override async Task<CrudValidationResult> ValidateCreateAsync(
        CurrencyCreateCommand command,
        CancellationToken cancellationToken)
    {
        var baseResult = await base.ValidateCreateAsync(command, cancellationToken);
        var errors = baseResult.Errors.ToList();

        if (await CurrencyRepository.IsExitedCurrencySymbol(command.Symbol))
            errors.Add(ErrorMessages.CurrencySymbolIsExisted);

        if (command.IsDefault)
        {
            var defaultCurrency = await CurrencyRepository.GetDefaultCurrency();
            if (defaultCurrency is not null)
                errors.Add(ErrorMessages.DefaultCurrencyIsAlreadyExistedWithName);
        }

        return errors.Count == 0
            ? CrudValidationResult.Success
            : CrudValidationResult.Failure(errors);
    }
}
