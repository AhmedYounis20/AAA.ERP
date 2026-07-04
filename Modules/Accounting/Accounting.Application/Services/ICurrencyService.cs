using Accounting.Application.Commands.Currencies;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Models.Entities.Account.Currencies;

namespace Accounting.Application.Services;

public interface ICurrencyService : IBaseSettingService<Currency, CurrencyCreateCommand, CurrencyUpdateCommand>;
