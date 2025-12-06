using ERP.Application.Data;
using ERP.Application.Services.Account;
using ERP.Application.Services.Caching;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.BaseEntities;
using System.Linq.Expressions;

namespace ERP.Infrastracture.Services.Caching;

/// <summary>
/// Query service with caching support for lookups
/// </summary>
public class CachedQueryService<TEntity, TDto> : IBaseQueryService<TEntity, TDto>
    where TEntity : BaseEntity
    where TDto : class
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICacheService _cacheService;
    private readonly string _cacheKeyPrefix;

    public CachedQueryService(
        IApplicationDbContext dbContext,
        ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
        _cacheKeyPrefix = typeof(TEntity).Name.ToLower();
    }

    public async Task<IEnumerable<TDto>> GetLookUps()
    {
        var cacheKey = CacheKeys.GetLookupsKey(_cacheKeyPrefix);

        var cached = await _cacheService.GetOrCreateAsync(
            cacheKey,
            async () =>
            {
                var data = await _dbContext.Set<TEntity>()
                    .AsNoTracking()
                    .Select(e => e.Adapt<TDto>())
                    .ToListAsync();
                return data;
            },
            CacheDurations.Medium);

        return cached ?? Enumerable.Empty<TDto>();
    }

    public async Task<IEnumerable<TDto>> GetLookUps(Expression<Func<TEntity, bool>> expression)
    {
        // For filtered lookups, we don't cache as the filter can vary
        // In production, you might want to cache common filter combinations
        return await _dbContext.Set<TEntity>()
            .AsNoTracking()
            .Where(expression)
            .Select(e => e.Adapt<TDto>())
            .ToListAsync();
    }

    /// <summary>
    /// Invalidates the lookup cache for this entity type
    /// </summary>
    public async Task InvalidateCacheAsync()
    {
        await _cacheService.RemoveByPrefixAsync(_cacheKeyPrefix);
    }
}

