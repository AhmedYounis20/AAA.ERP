using ERP.Application.Repositories.Account;
using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.Currencies;
using ERP.Domain.Models.Entities.Account.Currencies;
using ERP.Shared.Resources;
using Microsoft.Extensions.Localization;

namespace ERP.Infrastracture.Services.Account;

public class CurrencyService : BaseSettingService<Currency, CurrencyCreateCommand, CurrencyUpdateCommand>, ICurrencyService
{
    private readonly ICurrencyRepository _repository;
    private readonly IStringLocalizer<Resource> _localizer;
    private readonly IHttpContextAccessor _accessor;

    public CurrencyService(ICurrencyRepository repository, IStringLocalizer<Resource> localizer, IHttpContextAccessor accessor) : base(repository)
    {
        _repository = repository;
        _localizer = localizer;
        _accessor = accessor;
    }

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(CurrencyCreateCommand command)
    {

        var result = await base.ValidateCreate(command);

        var isExistedSymbol = await _repository.IsExitedCurrencySymbol(command.Symbol);
        if (isExistedSymbol)
        {
            result.isValid = false;
            result.errors.Add("CurrencySymbolIsExisted");

        }
        if (command.IsDefault)
        {
            Currency? defaultCurrency = await _repository.GetDefaultCurrency();
            if (defaultCurrency != null)
            {
                result.isValid = false;
                bool isArabic = _accessor.HttpContext.Request.Headers.ContainsKey("Accept-Language") &&
                                  _accessor.HttpContext.Request.Headers["Accept-Language"].Any(e => e.Contains("ar")) ||
                                          _accessor.HttpContext.Request.Headers.ContainsKey("Accept-Culture") &&
                                           _accessor.HttpContext.Request.Headers["Accept-Culture"].Any(e => e.Contains("ar"));
                result.errors.Add(_localizer["DefaultCurrencyIsAlreadyExistedWithName"].Value + " " + (isArabic ? defaultCurrency.Name : defaultCurrency.NameSecondLanguage));
            }
        }

        return result;
    }

    protected override async Task<(bool isValid, List<string> errors, Currency? entity)> ValidateUpdate(CurrencyUpdateCommand command)
    {
        var result = await base.ValidateUpdate(command);
        Currency? currencyWithSameSymbol =
            await _repository.GetQuery().Where(e => e.Symbol == command.Symbol).FirstOrDefaultAsync();
        if (currencyWithSameSymbol is not null && currencyWithSameSymbol.Id != command.Id)
        {
            result.isValid = false;
            result.errors.Add("CurrencySymbolIsExisted");
        }

        if (command.IsDefault)
        {
            Currency? defaultCurrency = await _repository.GetDefaultCurrency();
            if (defaultCurrency != null && defaultCurrency.Id != command.Id)
            {
                result.isValid = false;
                bool isArabic = _accessor.HttpContext.Request.Headers.ContainsKey("Accept-Language") &&
                                  _accessor.HttpContext.Request.Headers["Accept-Language"]
                                      .Any(e => e.Contains("ar")) ||
                                 _accessor.HttpContext.Request.Headers.ContainsKey("Accept-Culture") &&
                                  _accessor.HttpContext.Request.Headers["Accept-Culture"].Any(e => e.Contains("ar"));
                result.errors.Add(_localizer["DefaultCurrencyIsAlreadyExistedWithName"].Value + " " +
                                  (isArabic ? defaultCurrency.Name : defaultCurrency.NameSecondLanguage));
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