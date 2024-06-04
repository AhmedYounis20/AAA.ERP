using AAA.ERP.Models.Entities.AccountGuide;
using AAA.ERP.Models.Entities.GLSettings;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;

namespace AAA.ERP.Repositories.Interfaces;

public interface IGLSettingRepository{

    public Task<GLSetting?> Update(GLSetting setting);
    public Task<GLSetting?> GetGLSetting();
}