using Domain.Account.Models.Entities.GLSettings;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Account;

public class GLSettingRepository : BaseRepository<GLSetting>, IGLSettingRepository
{
    DbSet<GLSetting> _dbSet;
    public GLSettingRepository(ApplicationDbContext context) : base(context)
    => _dbSet = context.Set<GLSetting>();

    public async Task<GLSetting?> GetGLSetting()
    => await _dbSet.FirstOrDefaultAsync();
}