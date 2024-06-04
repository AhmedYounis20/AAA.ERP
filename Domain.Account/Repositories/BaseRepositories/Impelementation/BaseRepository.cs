using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.BaseEntities;
using Shared.BaseEntities.Identity;
using Shared.BaseRepositories.Interfaces;

namespace Domain.Account.Repositories.BaseRepositories.Impelementation;

public class BaseRepository<TEntity,TContext> 
    : IBaseRepository<TEntity,TContext> where TEntity : BaseEntity
    where TContext : IdentityDbContext<ApplicationUser>
{
    private TContext context;
    protected DbSet<TEntity> _dbSet;

    public BaseRepository(TContext _context)
    {
        context = _context;
        _dbSet = context.Set<TEntity>();
        context.GetType().GetProperties();
    }

    public virtual async Task<TEntity> Add(TEntity entity)
    {
        var entityEntry = await _dbSet.AddAsync(entity);
        await SaveChangesAsync();
        return entityEntry.Entity;
    }
    public virtual async Task Add(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await SaveChangesAsync();
    }
    
    public virtual async Task<TEntity?> Get(Guid id) 
    => await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
    public virtual async Task<IEnumerable<TEntity>> Get()
    => await _dbSet.ToListAsync(); // handle in future add pagination
    public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
    => await _dbSet.Where(predicate).ToListAsync();
    public IQueryable<TEntity> GetQuery()
     => _dbSet;

    public virtual async Task<TEntity?> Update(TEntity entity)
    {

        var result = _dbSet.Update(entity);
        await SaveChangesAsync();

        return result.Entity;
    }
    public virtual async Task Update(IEnumerable<TEntity> entities)
    {
        await Task.Run(() => { _dbSet.UpdateRange(entities); });
        await SaveChangesAsync();
    }
   
    public virtual async Task Delete(Guid entityId)
    {
        TEntity? entity = await Get(entityId);
        await Task.Run(() => { _dbSet.Remove(entity); });
        await SaveChangesAsync();
    }
    public virtual async Task Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
        await SaveChangesAsync();
    }
    public virtual async Task Delete(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
        await SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> GetTransaction() => await context.Database.BeginTransactionAsync();
    private async Task SaveChangesAsync() => await context.SaveChangesAsync();
    public void Dispose() => context.Dispose();

    // Exceptions
    public  async Task<bool> CheckIfInDatabase(Guid entityId)
       => await _dbSet.AnyAsync(e=>e.Id.Equals(entityId));

    public async Task<List<TEntity>> CheckIfInDatabase(IEnumerable<TEntity> entities)
    => await _dbSet.Where(x => entities.Select(e => e.Id).Contains(x.Id)).ToListAsync();

    //public virtual async Task<TEntity> UpdateWithAllIncludes(TEntity entity, int levels = 0)
    //{
    //    CheckNullParameter(entity);
    //    await CheckIfInDatabase(entity);

    //    await Task.Run(() => { dbSet.Update(entity); });
    //    await SaveChangesAsync();

    //    return await GetByIdWithAllIncludes(entity.Id, levels);
    //}
    //public virtual async Task<TEntity> UpdateWithCustomIncludes(TEntity entity, string includes)
    //{
    //    CheckNullParameter(entity);
    //    await CheckIfInDatabase(entity);

    //    await Task.Run(() => { dbSet.Update(entity); });
    //    await SaveChangesAsync();

    //    return await GetByIdWithCustomIncludes(entity.Id, includes);
    //}
    //public virtual async Task<TEntity> GetByIdWithCustomIncludes(Guid id, string includes)
    //{
    //    var query = dbSet.AsQueryable();
    //    foreach (var include in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
    //    {
    //        query = query.Include(include);
    //    }
    //    return await query.FirstOrDefaultAsync(e => e.Id == id) ?? Activator.CreateInstance<TEntity>();
    //}
    //public virtual async Task<IEnumerable<TEntity>> GetAllWithCustomIncludes(string includes)
    //{
    //    var query = dbSet.AsQueryable();
    //    foreach (var include in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
    //    {
    //        query = query.Include(include);
    //    }
    //    return await query.ToListAsync();
    //}
    //static public List<PropertyInfo> GetNavigationProp(Type typeofClass)
    //{

    //    List<PropertyInfo> navigationParamerters = new List<PropertyInfo>();
    //    foreach (var prop in typeofClass.GetProperties())
    //        navigationParamerters.Add(prop);

    //    return navigationParamerters.Where(e => e.PropertyType.IsClass && e.PropertyType != typeof(string)).ToList();

    //}
    //private static List<string> getParametersWithLevels(int level, Type classType, string ParentInclude = "")
    //{
    //    if (level == 0) return new List<string>();

    //    List<PropertyInfo> navigationParamerters = GetNavigationProp(classType);
    //    List<string> includes = new List<string>();

    //    foreach (var prop in navigationParamerters)
    //    {
    //        Type? propertyType = prop.PropertyType;

    //        includes.Add((ParentInclude + '.' + prop.Name).Substring(1));

    //        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
    //        {
    //            Type[] typeArguments = propertyType.GetGenericArguments();
    //            includes.AddRange(getParametersWithLevels(level - 1, typeArguments[0], ParentInclude + '.' + prop.Name));
    //        }
    //        else
    //            includes.AddRange(getParametersWithLevels(level - 1, propertyType, ParentInclude + '.' + prop.Name));
    //    }
    //    return includes;
    //}
    //public virtual async Task<IEnumerable<TEntity>> GetAllWithAllIncludes(int levels = 0)
    //{
    //    List<string> includes = getParametersWithLevels(levels, typeof(TEntity));
    //    var query = dbSet.AsQueryable();
    //    foreach (var include in includes)
    //    {
    //        query = query.Include(include);
    //    }
    //    return await query.ToListAsync();
    //}
    //public virtual async Task<TEntity> GetByIdWithAllIncludes(Guid id, int levels = 0)
    //{
    //    List<string> includes = getParametersWithLevels(levels, typeof(TEntity));
    //    var query = dbSet.AsQueryable();
    //    foreach (var include in includes)
    //    {
    //        query = query.Include(include);
    //    }
    //    return await query.FirstOrDefaultAsync(e => e.Id == id) ?? Activator.CreateInstance<TEntity>();
    //}
}

