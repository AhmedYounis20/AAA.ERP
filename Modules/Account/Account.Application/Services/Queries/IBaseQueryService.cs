using Shared.Application.BaseServices;
using Account.Domain.Commands.AccountGuides;
using Account.Domain.Models.Entities.AccountGuides;
using Shared.Domain.BaseEntities;
using System.Linq.Expressions;

namespace Account.Application.Services.Queries;

public interface IBaseQueryService<TEntity,TDto> where TEntity : BaseEntity where TDto : class{
    Task<IEnumerable<TDto>> GetLookUps();
    Task<IEnumerable<TDto>> GetLookUps(Expression<Func<TEntity, bool>> expression);
}