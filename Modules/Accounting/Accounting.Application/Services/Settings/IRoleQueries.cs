using Accounting.Domain.Models.Entities.Roles;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for role GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IRoleQueries : ISettingQueries<Role>;
