using ERP.BuildingBlocks.Application.Validators.Account.InputValidators.BaseValidators;
using Accounting.Domain.Commands;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.Currencies;

public class CurrencyInputValidator : BaseSettingInputValidator<CurrencyInputModel>
{
    public CurrencyInputValidator() : base()
    {
        _ = RuleFor(e => e.ExchangeRate).GreaterThan(0).WithMessage(ValidationMessages.ExchangeRateIsRequired);
        _ = RuleFor(e => e.ExchangeRate).Equal(1).When(e => e.IsDefault).WithMessage(ValidationMessages.ExchangeRateOnDefaultCurrency);
        _ = RuleFor(e => e.IsActive).Equal(true).When(e => e.IsDefault).WithMessage(ValidationMessages.CurrencyActiveOnDefault);

        _ = RuleFor(e => e.Symbol).MaximumLength(4).WithMessage(ValidationMessages.CurrencySymbolMaxLength);
    }
}
