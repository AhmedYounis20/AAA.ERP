using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Shared.BaseEntities;
using Shared.BaseEntities.Identity;

namespace Shared.BaseRepositories.Interfaces
{   
    public interface IBaseTreeRepository<TEntity,TContext> 
        : IBaseRepository<TEntity,TContext>,
            IDisposable where TEntity : BaseTreeEntity<TEntity>
        where TContext : IdentityDbContext<ApplicationUser>

    {
        Task<List<TEntity>> GetLevel(int level = 0 );
        Task<List<TEntity>> GetChildren(Guid id,int level= 0);
    }
}
