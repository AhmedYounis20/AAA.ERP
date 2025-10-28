using Account.Application.Repositories;
using Account.Domain.Models.Entities.GLSettings;
using Shared.Infrastructure.Repositories;

namespace Account.Infrastructure.Repositories;

public class GLSettingRepository : BaseRepository<GLSetting>, IGLSettingRepository
{
    DbSet<GLSetting> _dbSet;
    public GLSettingRepository(IAccountDbContext context) : base(context)
    => _dbSet = context.Set<GLSetting>();

    public async Task<GLSetting?> GetGLSetting()
    => await _dbSet.FirstOrDefaultAsync();
}
