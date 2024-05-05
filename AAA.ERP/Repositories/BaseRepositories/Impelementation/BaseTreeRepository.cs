using AAA.ERP.DBConfiguration.DbContext;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;

namespace AAA.ERP.Repositories.BaseRepositories.Impelementation;

public class BaseTreeRepository<TEntity> : BaseRepository<TEntity>, IBaseTreeRepository<TEntity> where TEntity : BaseTreeEntity<TEntity>
{
    public BaseTreeRepository(ApplicationDbContext context) : base(context)
    => _dbSet = context.Set<TEntity>();

    private async Task<TEntity> CheckIfInDatabase(Guid entityId)
    {
        TEntity? entity = await Get(entityId);
        if (entity == null)
            throw new ArgumentNullException($"{nameof(TEntity)} is not found in DB.");

        return entity;
    }
    public async Task<TEntity> CheckIfInDatabase(TEntity ent)
    {
        TEntity? entity = await Get(ent.Id);
        if (entity == null)
            throw new ArgumentNullException($"{nameof(TEntity)} is not found in DB.");

        return entity;
    }
    public async Task CheckIfInDatabase(IEnumerable<TEntity> entities)
    {
        foreach (TEntity entity in entities)
        {
            await CheckIfInDatabase(entity);
        }
    }

    public async Task<List<TEntity>> GetLevel(int level = 0)
    {
        List<TEntity> entities = new List<TEntity>();
        entities = await _dbSet.Where(e => e.ParentId == null).ToListAsync();
        if (level == 0)
            return entities;
        else
            return await GetChildren(entities, level - 1);
    }

    public async Task<List<TEntity>> GetChildren(List<TEntity> parents, int level = 0)
    {

        foreach (TEntity parent in parents)
            parent.Children = await GetChildren(parent.Id, level);

        return parents;
    }

    public async Task<List<TEntity>> GetChildren(Guid id, int level = 1)
    {
        List<TEntity> children = new List<TEntity>();

        children = await _dbSet.Where(e => e.ParentId.Equals(id)).ToListAsync();
        if (level == 0)
            return children;

        else
            return await GetChildren(children, level - 1);
    }
}