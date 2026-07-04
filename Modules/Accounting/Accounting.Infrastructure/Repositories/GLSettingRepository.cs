using Accounting.Application.Repositories;
using ERP.Domain.Models.Entities.Account.GLSettings;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace Accounting.Infrastructure.Repositories;

public class GLSettingRepository : BaseRepository<GLSetting>, IGLSettingRepository
{
    public GLSettingRepository(IApplicationDbContext context) : base(context) { }

    public async Task<GLSetting?> GetGLSetting()
        => await _dbSet.FirstOrDefaultAsync();
}
