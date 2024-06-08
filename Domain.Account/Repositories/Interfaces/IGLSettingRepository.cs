using Domain.Account.Models.Entities.GLSettings;

namespace Domain.Account.Repositories.Interfaces;

public interface IGLSettingRepository{

    public Task<GLSetting?> Update(GLSetting setting);
    public Task<GLSetting?> GetGLSetting();
}