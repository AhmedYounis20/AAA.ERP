using Account.Domain.Commands.Currencies;
using Account.Domain.Models.Entities.Currencies;
using Shared.Application.BaseServices;
using Shared.Domain.BaseEntities;
using Shared.Domain.Responses;

namespace Account.Application.Services;

public interface ICurrencyService : IBaseSettingService<Currency, CurrencyCreateCommand, CurrencyUpdateCommand> { }