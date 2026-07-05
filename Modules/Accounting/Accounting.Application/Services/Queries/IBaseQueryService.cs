using ERP.BuildingBlocks.Application.Services.BaseServices;
using ERP.BuildingBlocks.Domain.Entities;
using System.Linq.Expressions;

namespace Accounting.Application.Services.Queries;

/// <summary>
/// Application service contract for base query.
/// </summary>
public interface IBaseQueryService<TEntity, TDto> where TEntity : BaseEntity where TDto : class
{
    Task<IEnumerable<TDto>> GetLookUps();
    Task<IEnumerable<TDto>> GetLookUps(Expression<Func<TEntity, bool>> expression);
}
