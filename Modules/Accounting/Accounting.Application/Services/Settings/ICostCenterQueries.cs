using Accounting.Domain.Models.Entities.CostCenters;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for cost center GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ICostCenterQueries : ITreeSettingQueries<CostCenter>;
