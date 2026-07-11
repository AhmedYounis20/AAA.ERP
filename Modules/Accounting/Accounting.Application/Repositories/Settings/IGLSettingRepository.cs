using Accounting.Domain.Models.Entities.GLSettings;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for g l setting persistence.
/// </summary>
public interface IGLSettingRepository
{
    /// <summary>Updates entity.</summary>
    Task<GLSetting?> Update(GLSetting setting);
    /// <summary>Gets g l setting.</summary>
    Task<GLSetting?> GetGLSetting();
}
