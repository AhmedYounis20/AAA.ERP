﻿using ERP.Application.Validators.Account.InputValidators.BaseValidators;
using ERP.Domain.Commands.Account;
using FluentValidation;

namespace ERP.Application.Validators.Account.InputValidators;

public class CurrencyInputValidator : BaseSettingInputValidator<CurrencyInputModel>
{
    public CurrencyInputValidator():base() {

        _ = RuleFor(e => e.ExchangeRate).GreaterThan(0).WithMessage("ExchangeRateIsRequired");
        _ = RuleFor(e => e.ExchangeRate).Equal(1).When(e=>e.IsDefault).WithMessage("ExchangeRateOnDefaultCurrency");
        _ = RuleFor(e=>e.IsActive).Equal(true).When(e=>e.IsDefault).WithMessage("CurrencyActiveOnDefault");
        
        _ = RuleFor(e => e.Symbol).MaximumLength(4).WithMessage("CurrencySymbolMaxLength"); 
    }
}