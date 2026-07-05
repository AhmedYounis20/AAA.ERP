using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.GLSettings;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

public class GLSettingReadService(IGLSettingService glSettingService) : IGLSettingReadService
{
    public Task<ApiResponse<GLSetting>> Get() => glSettingService.Get();
}
