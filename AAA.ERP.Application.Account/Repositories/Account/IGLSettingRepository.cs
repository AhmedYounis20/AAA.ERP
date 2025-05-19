using Domain.Account.Models.Entities.GLSettings;

namespace ERP.Application.Repositories;

public interface IGLSettingRepository{

    public Task<GLSetting?> Update(GLSetting setting);
    public Task<GLSetting?> GetGLSetting();
}