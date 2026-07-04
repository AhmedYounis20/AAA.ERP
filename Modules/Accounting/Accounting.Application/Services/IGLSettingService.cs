using Accounting.Application.Commands.GLSettings;
using ERP.Domain.Models.Entities.Account.GLSettings;
using Shared.Responses;

namespace Accounting.Application.Services;

public interface IGLSettingService
{
    Task<ApiResponse<GLSetting>> Update(GlSettingUpdateCommand command);
    Task<ApiResponse<GLSetting>> Get();
}
