using Accounting.Application.Commands.Currencies;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Entities.Currencies;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for currency.
/// </summary>
public interface ICurrencyService : IBaseSettingService<Currency, CurrencyCreateCommand, CurrencyUpdateCommand>;
