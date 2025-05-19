using ERP.Domain.Commands.Account.GLSettings;
using ERP.Domain.Models.Entities.Account.GLSettings;
using Shared.Responses;

namespace ERP.Application.Services.Account;

public interface IGLSettingService
{

    public Task<ApiResponse<GLSetting>> Update(GlSettingUpdateCommand command);
    public Task<ApiResponse<GLSetting>> Get();
}