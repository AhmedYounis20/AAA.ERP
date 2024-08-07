using Domain.Account.Commands.AccountGuides;
using Domain.Account.Commands.Currencies;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Validators.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using FluentValidation;
using Shared.Responses;

namespace Domain.Account.Validators.ComandValidators.Currencies;

public class CurrencyCreateValidator : BaseSettingCreateValidator<CurrencyCreateCommand, Currency>
{
    public CurrencyCreateValidator() : base()
    {

        _ = RuleFor(e => e.ExchangeRate).GreaterThan(0).WithMessage("ExchangeRateGreaterThanZero");
        _ = RuleFor(e => e.ExchangeRate).Equal(1).When(e => e.IsDefault).WithMessage("ExchageRateOnDefaultCurrency");
        _ = RuleFor(e => e.IsActive).Equal(true).When(e => e.IsDefault).WithMessage("CurrencyActiveOnDefault");

        _ = RuleFor(e => e.Symbol).MaximumLength(4).WithMessage("CurrencySymbolMaxLength");
        _ = RuleFor(e => e.Symbol).NotEmpty().WithMessage("CurrencySymbolIsRequired");
    }
}