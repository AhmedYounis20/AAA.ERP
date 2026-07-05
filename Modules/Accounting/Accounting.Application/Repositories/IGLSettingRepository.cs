using Accounting.Domain.Models.Entities.GLSettings;

namespace Accounting.Application.Repositories;

public interface IGLSettingRepository
{
    Task<GLSetting?> Update(GLSetting setting);
    Task<GLSetting?> GetGLSetting();
}
