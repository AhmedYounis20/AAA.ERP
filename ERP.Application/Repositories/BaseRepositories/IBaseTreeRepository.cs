using Shared.BaseEntities;

namespace ERP.Application.Repositories.BaseRepositories
{
    public interface IBaseTreeRepository<TEntity>
        : IBaseRepository<TEntity>,
            IDisposable where TEntity : BaseTreeEntity<TEntity>

    {
        Task<List<TEntity>> GetLevel(int level = 0);
        Task<List<TEntity>> GetChildren(Guid id, int level = 0);
    }
}
