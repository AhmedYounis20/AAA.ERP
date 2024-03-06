using AAA.ERP.DBConfiguration.DbContext;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace AAA.ERP.Repositories.BaseRepositories.Impelementation;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    private ApplicationDbContext context;
    protected DbSet<TEntity> dbSet;

    public BaseRepository(ApplicationDbContext _context)
    {
        context = _context;
        dbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity> Add(TEntity entity)
    {
        var entityEntry = await dbSet.AddAsync(entity);
        await SaveChangesAsync();
        return entityEntry.Entity;
    }
    public virtual async Task Add(IEnumerable<TEntity> entities)
    {
        await dbSet.AddRangeAsync(entities);
        await SaveChangesAsync();
    }
    
    public virtual async Task<TEntity?> Get(Guid id) 
    => await dbSet.FirstOrDefaultAsync(e => e.Id == id);
    public virtual async Task<IEnumerable<TEntity>> Get()
    => await dbSet.ToListAsync(); // handle in future add pagination
    public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
    => await dbSet.Where(predicate).ToListAsync();
    public IQueryable<TEntity> GetQuery()
     => dbSet;

    public virtual async Task<TEntity?> Update(TEntity entity)
    {

        var result = dbSet.Update(entity);
        await SaveChangesAsync();

        return result.Entity;
    }
    public virtual async Task Update(IEnumerable<TEntity> entities)
    {
        await Task.Run(() => { dbSet.UpdateRange(entities); });
        await SaveChangesAsync();
    }
   
    public virtual async Task Delete(Guid entityId)
    {
        TEntity? entity = await CheckIfInDatabase(entityId);
        await Task.Run(() => { dbSet.Remove(entity); });
        await SaveChangesAsync();
    }
    public virtual async Task Delete(TEntity entity)
    {
        dbSet.Remove(entity);
        await SaveChangesAsync();
    }
    public virtual async Task Delete(IEnumerable<TEntity> entities)
    {
        dbSet.RemoveRange(entities);
        await SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> GetTransaction() => await context.Database.BeginTransactionAsync();
    private async Task SaveChangesAsync() => await context.SaveChangesAsync();
    public void Dispose() => context.Dispose();

    // Exceptions
    private void CheckNullParameter(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException($"{nameof(entity)} is not provided");
    }
    private void CheckNullParameter(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentNullException($"{nameof(TEntity)} was not provided");
    }
    private async Task<TEntity> CheckIfInDatabase(Guid entityId)
    {
        TEntity? entity = await Get(entityId);
        if (entity == null)
            throw new ArgumentNullException($"{nameof(TEntity)} is not found in DB.");

        return entity;
    }
    private async Task<TEntity> CheckIfInDatabase(TEntity ent)
    {
        TEntity? entity = await Get(ent.Id);
        if (entity == null)
            throw new ArgumentNullException($"{nameof(TEntity)} is not found in DB.");

        return entity;
    }
    private async Task CheckIfInDatabase(IEnumerable<TEntity> entities)
    {
        foreach (TEntity entity in entities)
        {
            await CheckIfInDatabase(entity);
        }
    }

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