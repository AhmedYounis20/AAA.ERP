using Accounting.Application.Commands.Currencies;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.Currencies;
using ERP.Infrastracture.Services.BaseServices;

namespace Accounting.Infrastructure.Services;

public class CurrencyService : BaseSettingService<Currency, CurrencyCreateCommand, CurrencyUpdateCommand>, ICurrencyService
{
    private readonly ICurrencyRepository _repository;
    private readonly IHttpContextAccessor _accessor;

    public CurrencyService(ICurrencyRepository repository, IHttpContextAccessor accessor) : base(repository)
    {
        _repository = repository;
        _accessor = accessor;
    }

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(CurrencyCreateCommand command)
    {
        var result = await base.ValidateCreate(command);

        if (await _repository.IsExitedCurrencySymbol(command.Symbol))
        {
            result.isValid = false;
            result.errors.Add("CurrencySymbolIsExisted");
        }

        if (command.IsDefault)
        {
            var defaultCurrency = await _repository.GetDefaultCurrency();
            if (defaultCurrency != null)
            {
                result.isValid = false;
                result.errors.Add("DefaultCurrencyIsAlreadyExistedWithName");
            }
        }

        return result;
    }

    protected override async Task<(bool isValid, List<string> errors, Currency? entity)> ValidateUpdate(CurrencyUpdateCommand command)
    {
        var result = await base.ValidateUpdate(command);

        var currencyWithSameSymbol = await _repository.GetQuery()
            .Where(e => e.Symbol == command.Symbol)
            .FirstOrDefaultAsync();

        if (currencyWithSameSymbol is not null && currencyWithSameSymbol.Id != command.Id)
        {
            result.isValid = false;
            result.errors.Add("CurrencySymbolIsExisted");
        }

        if (command.IsDefault)
        {
            var defaultCurrency = await _repository.GetDefaultCurrency();
            if (defaultCurrency != null && defaultCurrency.Id != command.Id)
            {
                result.isValid = false;
                result.errors.Add("DefaultCurrencyIsAlreadyExistedWithName");
            }
        }

        return result;
    }

    protected override async Task<(bool isValid, List<string> errors, Currency? entity)> ValidateDelete(Guid id)
    {
        var result = await base.ValidateDelete(id);
        if (result.entity is not null && result.entity.IsDefault)
        {
            result.isValid = false;
            result.errors.Add("DefaultCurrencyCannotBeDeleted");
        }

        return result;
    }
}
