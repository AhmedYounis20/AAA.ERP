using Accounting.Application.Commands.GLSettings;
using Accounting.Domain.Models.Entities.GLSettings;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for g l setting.
/// </summary>
public interface IGLSettingService
{
    Task<ApiResponse<GLSetting>> Update(GlSettingUpdateCommand command);
    Task<ApiResponse<GLSetting>> Get();
}
