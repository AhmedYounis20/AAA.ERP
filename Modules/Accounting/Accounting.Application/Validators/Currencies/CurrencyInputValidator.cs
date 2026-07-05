using ERP.BuildingBlocks.Application.Validators.Account.InputValidators.BaseValidators;
using Accounting.Domain.Commands;
using FluentValidation;
using Shared.Constants;

namespace Accounting.Application.Validators.Currencies;

public class CurrencyInputValidator : BaseSettingInputValidator<CurrencyInputModel>
{
    public CurrencyInputValidator() : base()
    {
        _ = RuleFor(e => e.ExchangeRate).GreaterThan(0).WithMessage(ErrorMessages.ExchangeRateIsRequired);
        _ = RuleFor(e => e.ExchangeRate).Equal(1).When(e => e.IsDefault).WithMessage(ErrorMessages.ExchangeRateOnDefaultCurrency);
        _ = RuleFor(e => e.IsActive).Equal(true).When(e => e.IsDefault).WithMessage(ErrorMessages.CurrencyActiveOnDefault);

        _ = RuleFor(e => e.Symbol).MaximumLength(4).WithMessage(ErrorMessages.CurrencySymbolMaxLength);
    }
}
