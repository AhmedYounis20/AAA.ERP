using Domain.Account.Models.Entities.GLSettings;
using Shared.Responses;

namespace Domain.Account.Services.Interfaces;

public interface IGLSettingService{

    public Task<ApiResponse<GLSetting>> Update(GLSetting glsettting);
    public Task<ApiResponse<GLSetting>> Get();
}