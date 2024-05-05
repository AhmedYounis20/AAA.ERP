using AAA.ERP.Models.BaseEntities;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace AAA.ERP.Repositories.BaseRepositories.Interfaces
{   
    public interface IBaseTreeRepository<TEntity> : IBaseRepository<TEntity>, IDisposable where TEntity : BaseTreeEntity<TEntity>
    {
        Task<List<TEntity>> GetLevel(int level = 0 );
        Task<List<TEntity>> GetChildren(Guid id,int level= 0);
    }
}
