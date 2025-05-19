using Domain.Account.Commands.Currencies;
using Domain.Account.Models.Entities.Currencies;
using ERP.Application.Services.BaseServices;

namespace ERP.Application.Services.Account;

public interface ICurrencyService : IBaseSettingService<Currency, CurrencyCreateCommand, CurrencyUpdateCommand> { }