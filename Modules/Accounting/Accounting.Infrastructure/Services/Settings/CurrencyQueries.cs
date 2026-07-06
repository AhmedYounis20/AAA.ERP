using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Currencies;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Currency read queries.
/// </summary>
public class CurrencyQueries(ICurrencyRepository repository)
    : BaseSettingRepositoryQueries<Currency>(repository),
      ICurrencyQueries;
