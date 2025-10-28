using Account.Domain.Commands.Currencies;
using Account.Domain.Models.Entities.Currencies;
using FluentValidation;
using Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;
using Shared.Domain.BaseEntities;

namespace Account.Application.Validators.CommandValidators.Currencies;

public class CurrencyCreateValidator : BaseSettingCreateValidator<CurrencyCreateCommand, Currency>
{
    public CurrencyCreateValidator() : base()
    {

        _ = RuleFor(e => e.ExchangeRate).GreaterThan(0).WithMessage("ExchangeRateGreaterThanZero");
        _ = RuleFor(e => e.ExchangeRate).Equal(1).When(e => e.IsDefault).WithMessage("ExchangeRateOnDefaultCurrency");
        _ = RuleFor(e => e.IsActive).Equal(true).When(e => e.IsDefault).WithMessage("CurrencyActiveOnDefault");

        _ = RuleFor(e => e.Symbol).MaximumLength(4).WithMessage("CurrencySymbolMaxLength");
        _ = RuleFor(e => e.Symbol).NotEmpty().WithMessage("CurrencySymbolIsRequired");
    }
}