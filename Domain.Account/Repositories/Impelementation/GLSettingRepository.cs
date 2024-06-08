using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.GLSettings;
using Domain.Account.Repositories.BaseRepositories.Impelementation;
using Domain.Account.Repositories.Interfaces;

namespace Domain.Account.Repositories.Impelementation;

public class GLSettingRepository : BaseRepository<GLSetting>, IGLSettingRepository
{
    DbSet<GLSetting> _dbSet;
    public GLSettingRepository(ApplicationDbContext context) : base(context)
    => _dbSet = context.Set<GLSetting>();

    public async Task<GLSetting?> GetGLSetting()
    => await _dbSet.FirstOrDefaultAsync();
}