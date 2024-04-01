using AAA.ERP.DBConfiguration.DbContext;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;

namespace AAA.ERP.Repositories.BaseRepositories.Impelementation;

public class BaseTreeSettingRepository<TEntity> : BaseTreeRepository<TEntity>, IBaseTreeSettingRepository<TEntity> where TEntity : BaseTreeSettingEntity
{
    private ApplicationDbContext _context;

    public BaseTreeSettingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        dbSet = context.Set<TEntity>();
    }

    public async Task<bool> AnyByNames(string? name, string? nameSecondLanguage)
    {
        string normalizedName = name?.Trim().ToUpper();
        string normalizedSecondLanguageName = nameSecondLanguage?.Trim().ToUpper();

        // Check if both strings are non-null and then perform the query
        if (!string.IsNullOrEmpty(normalizedName) && !string.IsNullOrEmpty(normalizedSecondLanguageName))
        {
            // Use AnyAsync with a case-insensitive comparison
            bool exists = await dbSet.AnyAsync(e =>
                e.Name.ToUpper() == normalizedName ||
                e.NameSecondLanguage.ToUpper() == normalizedSecondLanguageName
            );

            // Use the exists boolean for further operations
            return exists;
        }
        return false;
    }

    public async Task<TEntity?> GetByNames(string? name, string? nameSecondLanguage)
    {
        string normalizedName = name?.Trim().ToUpper();
        string normalizedSecondLanguageName = nameSecondLanguage?.Trim().ToUpper();
        TEntity? entity = null;
        // Check if both strings are non-null and then perform the query
        if (!string.IsNullOrEmpty(normalizedName) && !string.IsNullOrEmpty(normalizedSecondLanguageName))
        {
            // Use AnyAsync with a case-insensitive comparison
            var exists = await dbSet.Where(e =>
                e.Name.ToUpper() == normalizedName ||
                e.NameSecondLanguage.ToUpper() == normalizedSecondLanguageName
            ).FirstOrDefaultAsync();

            // Use the exists boolean for further operations
            return exists;
        }
        return entity;
    }

    public async Task<IEnumerable<TEntity>> Search(string name)
    {
        name = name.ToUpper();
        var result = await dbSet.Where(x =>
        (!string.IsNullOrEmpty(x.Name) && x.Name.ToUpper().Contains(name))
        || (!string.IsNullOrEmpty(x.NameSecondLanguage) && x.NameSecondLanguage.ToUpper().Contains(name))
        ).ToListAsync();

        return result;
    }
}