using Account.Domain.Models.Entities.GLSettings;

namespace Account.Application.Repositories;

public interface IGLSettingRepository
{

    public Task<GLSetting?> Update(GLSetting setting);
    public Task<GLSetting?> GetGLSetting();
}