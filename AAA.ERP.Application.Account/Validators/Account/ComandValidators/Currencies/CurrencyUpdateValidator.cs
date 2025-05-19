using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using ERP.Domain.Commands.Account.Currencies;
using ERP.Domain.Models.Entities.Account.Currencies;
using FluentValidation;

namespace ERP.Application.Validators.Account.ComandValidators.Currencies;

public class CurrencyUpdateValidator : BaseSettingUpdateValidator<CurrencyUpdateCommand, Currency>
{
    public CurrencyUpdateValidator() : base()
    {

        _ = RuleFor(e => e.ExchangeRate).GreaterThan(0).WithMessage("ExchangeRateIsRequired");
        _ = RuleFor(e => e.ExchangeRate).Equal(1).When(e => e.IsDefault).WithMessage("ExchangeRateOnDefaultCurrency");
        _ = RuleFor(e => e.IsActive).Equal(true).When(e => e.IsDefault).WithMessage("CurrencyActiveOnDefault");

        _ = RuleFor(e => e.Symbol).MaximumLength(4).WithMessage("CurrencySymbolMaxLength");
        _ = RuleFor(e => e.Symbol).NotEmpty().WithMessage("CurrencySymbolIsRequired");

    }
}