using AAA.ERP.Models.Data.Currencies;
using AAA.ERP.Repositories.Interfaces;
using AAA.ERP.Resources;
using AAA.ERP.Validators.BussinessValidator.Interfaces;
using Microsoft.Extensions.Localization;

namespace AAA.ERP.Validators.BussinessValidator;

public class CurrencyBussinessValidator : BaseSettingBussinessValidator<Currency>, ICurrencyBussinessValidator
{
    private ICurrencyRepository _repository;
    private IStringLocalizer _stringLocalizer;
    public CurrencyBussinessValidator(ICurrencyRepository repository,IStringLocalizer<Resource> localizer) : base(repository, localizer)
    {
        _repository = repository;
        _stringLocalizer = localizer;
    }

    public override async Task<(bool IsValid, List<string> ListOfErrors, Currency? entity)> ValidateCreateBussiness(Currency inpuModel)
    {
        bool isValid = false;
        List<string> listOfErrors = new List<string>();
        Currency? currency = null;

        (isValid, listOfErrors, currency) = await base.ValidateCreateBussiness(inpuModel);

        var isExistedSymbol = await _repository.IsExitedCurrencySymbol(inpuModel.Symbol);
        if (isExistedSymbol)
        {
            isValid = false;
            listOfErrors.Add("CurrencySymbolIsExisted");

        }
        if (inpuModel.IsDefault)
        {
            Currency? defaultCurrency = await _repository.GetDefaultCurrency();
            if(defaultCurrency != null)
            {
                isValid = false;
                var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
                listOfErrors.Add(_stringLocalizer["DefaultCurrencyIsAlreadyExitedWithName"].Value +" "+ (currentCulture.Name=="ar-EG" ? defaultCurrency.Name : defaultCurrency.NameSecondLanguage));
            }
        }


        return (isValid, listOfErrors, currency);
    }
}