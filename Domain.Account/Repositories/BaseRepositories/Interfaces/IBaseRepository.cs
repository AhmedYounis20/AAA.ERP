using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Shared.BaseEntities;
using Shared.BaseEntities.Identity;

namespace Shared.BaseRepositories.Interfaces
{
    public interface IBaseRepository<TEntity,TContext> 
        : IDisposable where TEntity : BaseEntity
    where TContext : IdentityDbContext<ApplicationUser>
    {
        public Task<TEntity> Add(TEntity entity);
        public Task Add(IEnumerable<TEntity> entities);

        public Task<TEntity?> Get(Guid id);
        public Task<IEnumerable<TEntity>> Get();
        public IQueryable<TEntity> GetQuery();
        public Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);

        public Task<TEntity?> Update(TEntity entity);
        public Task Update(IEnumerable<TEntity> entities);

        public Task Delete(Guid entityId);
        public Task Delete(TEntity entity);
        public Task Delete(IEnumerable<TEntity> entities);

        public Task<bool> CheckIfInDatabase(Guid entityId);
        public Task<List<TEntity>> CheckIfInDatabase(IEnumerable<TEntity> entities);
    }
}
