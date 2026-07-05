using Accounting.Application.Commands.Currencies;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Currencies;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Currency read queries. Transitional: delegates to <see cref="ICurrencyService"/> until write methods are removed.
/// </summary>
public class CurrencyQueries(ICurrencyService currencyService)
    : BaseSettingQueries<Currency, CurrencyCreateCommand, CurrencyUpdateCommand>(currencyService),
      ICurrencyQueries;
