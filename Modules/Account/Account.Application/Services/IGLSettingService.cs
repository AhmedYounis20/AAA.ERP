using Account.Domain.Commands.GLSettings;
using Account.Domain.Models.Entities.GLSettings;
using Shared.Domain.Responses;

namespace Account.Application.Services;

public interface IGLSettingService
{

    public Task<ApiResponse<GLSetting>> Update(GlSettingUpdateCommand command);
    public Task<ApiResponse<GLSetting>> Get();
}