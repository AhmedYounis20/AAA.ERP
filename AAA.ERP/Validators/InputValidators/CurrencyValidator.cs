using AAA.ERP.InputModels;
using AAA.ERP.Validators.InputValidators.BaseValidators;
using FluentValidation;

namespace AAA.ERP.Validators.InputValidators;

public class CurrencyValidator : BaseSettingInputValidator<CurrencyInputModel>
{
    public CurrencyValidator() {

        _ = RuleFor(e => e.ExchangeRate).GreaterThan(0).WithMessage("ExchangeRateIsRequired");
        _ = RuleFor(e => e.ExchangeRate).Equal(1).When(e=>e.IsDefault).WithMessage("ExchageRateOnDefaultCurrency");
        _ = RuleFor(e=>e.IsActive).Equal(true).When(e=>e.IsDefault).WithMessage("CurrencyActiveOnDefault");
        
        _ = RuleFor(e => e.Symbol).MaximumLength(4).WithMessage("CurrencySymbolMaxLength"); 
    }
}