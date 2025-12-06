using Shared.DTOs;

namespace ERP.Infrastracture.Repositories.BaseRepositories;

public class BaseRepository<TEntity>
    : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    private IApplicationDbContext context;
    protected DbSet<TEntity> _dbSet;

    public BaseRepository(IApplicationDbContext _context)
    {
        context = _context;
        _dbSet = context.Set<TEntity>();
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

    public virtual async Task<TEntity?> Get(Guid? id)
    => await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
    public virtual async Task<IEnumerable<TEntity>> Get()
    => await _dbSet.ToListAsync(); // handle in future add pagination
    public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
    => await _dbSet.Where(predicate).ToListAsync();
    public DbSet<TEntity> GetQuery()
     => _dbSet;

    public virtual async Task<PaginatedResult<TEntity>> GetPaginated(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool descending = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        if (filter != null)
            query = query.Where(filter);

        if (orderBy != null)
            query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        else
            query = query.OrderByDescending(e => e.CreatedAt);

        return await query.ToPaginatedResultAsync(pageNumber, pageSize, cancellationToken);
    }

    public virtual async Task<PaginatedResult<TDto>> GetPaginated<TDto>(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, TDto>> selector,
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool descending = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        if (filter != null)
            query = query.Where(filter);

        if (orderBy != null)
            query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        else
            query = query.OrderByDescending(e => e.CreatedAt);

        var projectedQuery = query.Select(selector);
        return await projectedQuery.ToPaginatedResultAsync(pageNumber, pageSize, cancellationToken);
    }

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

    public virtual async Task<TEntity?> Delete(Guid entityId)
    {
        TEntity? entity = await Get(entityId);
        await Task.Run(() => { _dbSet.Remove(entity); });
        await SaveChangesAsync();
        return entity;
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
    public async Task<bool> CheckIfInDatabase(Guid entityId)
       => await _dbSet.AnyAsync(e => e.Id.Equals(entityId));

    public async Task<List<TEntity>> CheckIfInDatabase(IEnumerable<TEntity> entities)
    => await _dbSet.Where(x => entities.Select(e => e.Id).Contains(x.Id)).ToListAsync();

    #region Bulk Operations

    public virtual async Task BulkInsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        const int batchSize = 1000;
        var entityList = entities.ToList();

        for (int i = 0; i < entityList.Count; i += batchSize)
        {
            var batch = entityList.Skip(i).Take(batchSize);
            await _dbSet.AddRangeAsync(batch, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task BulkUpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        const int batchSize = 1000;
        var entityList = entities.ToList();

        for (int i = 0; i < entityList.Count; i += batchSize)
        {
            var batch = entityList.Skip(i).Take(batchSize);
            _dbSet.UpdateRange(batch);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task BulkDeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        const int batchSize = 1000;
        var entityList = entities.ToList();

        for (int i = 0; i < entityList.Count; i += batchSize)
        {
            var batch = entityList.Skip(i).Take(batchSize);
            _dbSet.RemoveRange(batch);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task BulkDeleteByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        const int batchSize = 1000;
        var idList = ids.ToList();

        for (int i = 0; i < idList.Count; i += batchSize)
        {
            var batchIds = idList.Skip(i).Take(batchSize).ToList();
            var entitiesToDelete = await _dbSet.Where(e => batchIds.Contains(e.Id)).ToListAsync(cancellationToken);
            _dbSet.RemoveRange(entitiesToDelete);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken);
    }

    public virtual async Task<int> ExecuteUpdateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TEntity>> updateExpression,
        CancellationToken cancellationToken = default)
    {
        // Note: EF Core 7+ supports ExecuteUpdateAsync with SetProperty
        // For complex updates, we use the traditional approach
        var entities = await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        var compiled = updateExpression.Compile();
        
        foreach (var entity in entities)
        {
            var updated = compiled(entity);
            _dbSet.Entry(entity).CurrentValues.SetValues(updated);
        }
        
        return await context.SaveChangesAsync(cancellationToken);
    }

    #endregion

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

