using AAA.ERP.DBConfiguration.DbContext;
using AAA.ERP.Models.Entities.AccountGuide;
using AAA.ERP.Models.Entities.GLSettings;
using AAA.ERP.Repositories.BaseRepositories.Impelementation;
using AAA.ERP.Repositories.Interfaces;

namespace AAA.ERP.Repositories.Impelementation;

public class GLSettingRepository : BaseRepository<GLSetting>, IGLSettingRepository
{
    DbSet<GLSetting> _dbSet;
    public GLSettingRepository(ApplicationDbContext context) : base(context)
    => _dbSet = context.Set<GLSetting>();

    public async Task<GLSetting?> GetGLSetting()
    => await _dbSet.FirstOrDefaultAsync();
}