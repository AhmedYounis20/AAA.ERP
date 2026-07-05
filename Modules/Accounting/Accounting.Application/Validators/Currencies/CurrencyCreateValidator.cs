using Accounting.Application.Commands.Currencies;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Domain.Models.Entities.Currencies;
using FluentValidation;
using Shared.Constants;

namespace Accounting.Application.Validators.Currencies;

public class CurrencyCreateValidator : BaseSettingCreateValidator<CurrencyCreateCommand, Currency>
{
    public CurrencyCreateValidator() : base()
    {
        _ = RuleFor(e => e.ExchangeRate).GreaterThan(0).WithMessage(ErrorMessages.ExchangeRateGreaterThanZero);
        _ = RuleFor(e => e.ExchangeRate).Equal(1).When(e => e.IsDefault).WithMessage(ErrorMessages.ExchangeRateOnDefaultCurrency);
        _ = RuleFor(e => e.IsActive).Equal(true).When(e => e.IsDefault).WithMessage(ErrorMessages.CurrencyActiveOnDefault);

        _ = RuleFor(e => e.Symbol).MaximumLength(4).WithMessage(ErrorMessages.CurrencySymbolMaxLength);
        _ = RuleFor(e => e.Symbol).NotEmpty().WithMessage(ErrorMessages.CurrencySymbolIsRequired);
    }
}
