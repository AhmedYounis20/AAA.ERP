using Domain.Account.Commands.Currencies;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces;

namespace Domain.Account.Services.Impelementation;

public class CurrencyService : BaseSettingService<Currency,CurrencyCreateCommand,CurrencyUpdateCommand>, ICurrencyService
{
    private readonly ICurrencyRepository _repository;

    public CurrencyService(ICurrencyRepository repository) : base(repository)
        => _repository = repository;

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(CurrencyCreateCommand command)
    {
        
        var result  = await base.ValidateCreate(command);

        var isExistedSymbol = await _repository.IsExitedCurrencySymbol(command.Symbol);
        if (isExistedSymbol)
        {
            result.isValid = false;
            result.errors.Add("CurrencySymbolIsExisted");

        }
        if (command.IsDefault)
        {
            Currency? defaultCurrency = await _repository.GetDefaultCurrency();
            if (defaultCurrency != null)
            {
                result.isValid = false;
                result.errors.Add("DefaultCurrencyIsAlreadyExitedWithName");
            }
        }

        return result;
    }
} 