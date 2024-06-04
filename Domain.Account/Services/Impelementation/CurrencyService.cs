﻿using AAA.ERP.Models.Entities.Currencies;
using AAA.ERP.Services.BaseServices.impelemtation;
using AAA.ERP.Repositories.Interfaces;
using AAA.ERP.Services.Interfaces;
using AAA.ERP.Validators.BussinessValidator.Interfaces;
using AAA.ERP.Responses;

namespace AAA.ERP.Services.Impelementation;

public class CurrencyService : BaseSettingService<Currency>, ICurrencyService
{
    public CurrencyService(ICurrencyRepository repository,
                           ICurrencyBussinessValidator bussinessValidator) : base(repository, bussinessValidator)
    { }
}