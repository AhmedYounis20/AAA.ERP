using Domain.Account.Repositories.BaseRepositories.Impelementation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.BaseEntities;
using Shared.BaseEntities.Identity;
using Shared.BaseRepositories.Interfaces;

namespace Shared.BaseRepositories.Impelementation;

public class BaseSettingRepository<TEntity,TContext> 
    : BaseRepository<TEntity,TContext>,
        IBaseSettingRepository<TEntity,TContext> 
        where TEntity : BaseSettingEntity
        where TContext : IdentityDbContext<ApplicationUser>

{
    private TContext _context;

    public BaseSettingRepository(TContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<bool> AnyByNames(string? name, string? nameSecondLanguage)
    {
        string normalizedName = name?.Trim().ToUpper();
        string normalizedSecondLanguageName = nameSecondLanguage?.Trim().ToUpper();

        // Check if both strings are non-null and then perform the query
        if (!string.IsNullOrEmpty(normalizedName) && !string.IsNullOrEmpty(normalizedSecondLanguageName))
        {
            // Use AnyAsync with a case-insensitive comparison
            bool exists = await _dbSet.AnyAsync(e =>
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
            var exists = await _dbSet.Where(e =>
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
        var result = await _dbSet.Where(x =>
        (!string.IsNullOrEmpty(x.Name) && x.Name.ToUpper().Contains(name))
        || (!string.IsNullOrEmpty(x.NameSecondLanguage) && x.NameSecondLanguage.ToUpper().Contains(name))
        ).ToListAsync();

        return result;
    }
}