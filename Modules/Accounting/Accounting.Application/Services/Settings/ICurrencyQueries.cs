using Accounting.Domain.Models.Entities.Currencies;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for currency GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ICurrencyQueries : ISettingQueries<Currency>;
