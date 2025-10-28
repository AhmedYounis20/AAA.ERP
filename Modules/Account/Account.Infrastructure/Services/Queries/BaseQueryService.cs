using Account.Application.Services.Queries;

namespace Account.Infrastructure.Services.Queries;
public class BaseQueryService<TEntity, TDto> : IBaseQueryService<TEntity, TDto> where TEntity : BaseEntity where TDto : class
{
    IAccountDbContext _dbContext;
    public BaseQueryService(IAccountDbContext applicationDbContext)
    {
        _dbContext = applicationDbContext;
    }

    public async Task<IEnumerable<TDto>> GetLookUps()
    {
        return await _dbContext.Set<TEntity>().Select(e=>e.Adapt<TDto>()).ToListAsync();
    }

    public async Task<IEnumerable<TDto>> GetLookUps(Expression<Func<TEntity,bool>> expression)
    {
        return await _dbContext.Set<TEntity>().Where(expression).Select(e => e.Adapt<TDto>()).ToListAsync();
    }
}
