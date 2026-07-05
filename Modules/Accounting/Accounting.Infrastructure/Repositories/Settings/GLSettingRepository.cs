using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.GLSettings;
using ERP.Infrastructure.Repositories.BaseRepositories;

namespace Accounting.Infrastructure.Repositories;

public class GLSettingRepository : BaseRepository<GLSetting>, IGLSettingRepository
{
    public GLSettingRepository(IApplicationDbContext context) : base(context) { }

    public async Task<GLSetting?> GetGLSetting()
        => await _dbSet.FirstOrDefaultAsync();
}
