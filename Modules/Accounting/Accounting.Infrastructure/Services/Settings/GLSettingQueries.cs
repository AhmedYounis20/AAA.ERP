using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.GLSettings;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// GL setting read queries. Transitional: delegates to <see cref="IGLSettingService"/>.
/// </summary>
public class GLSettingQueries(IGLSettingService glSettingService) : IGLSettingQueries
{
    public Task<ApiResponse<GLSetting>> Get() => glSettingService.Get();
}
