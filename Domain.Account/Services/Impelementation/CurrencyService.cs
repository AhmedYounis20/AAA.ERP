using AAA.ERP.Validators.BussinessValidator.Interfaces;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces;

namespace Domain.Account.Services.Impelementation;

public class CurrencyService : BaseSettingService<Currency>, ICurrencyService
{
    public CurrencyService(ICurrencyRepository repository,
                           ICurrencyBussinessValidator bussinessValidator) : base(repository, bussinessValidator)
    { }
}