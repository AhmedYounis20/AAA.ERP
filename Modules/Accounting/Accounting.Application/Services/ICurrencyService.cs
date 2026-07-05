using Accounting.Application.Commands.Currencies;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Entities.Currencies;

namespace Accounting.Application.Services;

public interface ICurrencyService : IBaseSettingService<Currency, CurrencyCreateCommand, CurrencyUpdateCommand>;
