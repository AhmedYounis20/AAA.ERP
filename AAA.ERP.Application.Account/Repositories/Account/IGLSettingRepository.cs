using ERP.Domain.Models.Entities.Account.GLSettings;

namespace ERP.Application.Repositories.Account;

public interface IGLSettingRepository
{

    public Task<GLSetting?> Update(GLSetting setting);
    public Task<GLSetting?> GetGLSetting();
}