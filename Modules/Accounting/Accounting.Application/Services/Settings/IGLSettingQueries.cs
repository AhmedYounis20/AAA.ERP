using Accounting.Domain.Models.Entities.GLSettings;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for GL setting GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IGLSettingQueries
{
    Task<ApiResponse<GLSetting>> Get();
}
