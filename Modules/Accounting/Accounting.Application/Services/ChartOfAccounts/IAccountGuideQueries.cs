using Accounting.Domain.Models.Entities.AccountGuides;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for account guide GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IAccountGuideQueries : ISettingQueries<AccountGuide>;
