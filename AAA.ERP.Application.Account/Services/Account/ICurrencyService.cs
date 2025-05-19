using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Account.Currencies;
using ERP.Domain.Models.Entities.Account.Currencies;

namespace ERP.Application.Services.Account;

public interface ICurrencyService : IBaseSettingService<Currency, CurrencyCreateCommand, CurrencyUpdateCommand> { }