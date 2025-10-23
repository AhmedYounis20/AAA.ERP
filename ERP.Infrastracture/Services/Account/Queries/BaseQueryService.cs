using ERP.Application.Repositories.Account;
using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.AccountGuides;
using ERP.Domain.Models.Entities.Account.AccountGuides;
using System.Linq;

namespace ERP.Infrastracture.Services.Account.Queries;
public class BaseQueryService<TEntity, TDto> : IBaseQueryService<TEntity, TDto> where TEntity : BaseEntity where TDto : class
{
    IApplicationDbContext _dbContext;
    public BaseQueryService(IApplicationDbContext applicationDbContext)
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