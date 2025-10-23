using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Account.AccountGuides;
using ERP.Domain.Models.Entities.Account.AccountGuides;
using Shared.BaseEntities;
using System.Linq.Expressions;

namespace ERP.Application.Services.Account;

public interface IBaseQueryService<TEntity,TDto> where TEntity : BaseEntity where TDto : class{
    Task<IEnumerable<TDto>> GetLookUps();
    Task<IEnumerable<TDto>> GetLookUps(Expression<Func<TEntity, bool>> expression);
}