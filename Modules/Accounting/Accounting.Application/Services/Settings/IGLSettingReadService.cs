using Accounting.Domain.Models.Entities.GLSettings;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

public interface IGLSettingReadService
{
    Task<ApiResponse<GLSetting>> Get();
}
