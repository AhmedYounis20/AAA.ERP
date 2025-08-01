﻿using Microsoft.EntityFrameworkCore;
using Shared.BaseEntities;
using System.Linq.Expressions;

namespace ERP.Application.Repositories.BaseRepositories
{
    public interface IBaseRepository<TEntity>
        : IDisposable where TEntity : BaseEntity
    {
        public Task<TEntity> Add(TEntity entity);
        public Task Add(IEnumerable<TEntity> entities);

        public Task<TEntity?> Get(Guid? id);
        public Task<IEnumerable<TEntity>> Get();
        public DbSet<TEntity> GetQuery();
        public Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);

        public Task<TEntity?> Update(TEntity entity);
        public Task Update(IEnumerable<TEntity> entities);

        public Task<TEntity?> Delete(Guid entityId);
        public Task Delete(TEntity entity);
        public Task Delete(IEnumerable<TEntity> entities);

        public Task<bool> CheckIfInDatabase(Guid entityId);
        public Task<List<TEntity>> CheckIfInDatabase(IEnumerable<TEntity> entities);
    }
}
