using Microsoft.EntityFrameworkCore;
using Shared.BaseEntities;
using Shared.DTOs;
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

        /// <summary>
        /// Gets paginated results with optional filtering and sorting
        /// </summary>
        public Task<PaginatedResult<TEntity>> GetPaginated(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>>? filter = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool descending = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets paginated results with projection to DTO
        /// </summary>
        public Task<PaginatedResult<TDto>> GetPaginated<TDto>(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, TDto>> selector,
            Expression<Func<TEntity, bool>>? filter = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool descending = false,
            CancellationToken cancellationToken = default);

        public Task<TEntity?> Update(TEntity entity);
        public Task Update(IEnumerable<TEntity> entities);

        public Task<TEntity?> Delete(Guid entityId);
        public Task Delete(TEntity entity);
        public Task Delete(IEnumerable<TEntity> entities);

        public Task<bool> CheckIfInDatabase(Guid entityId);
        public Task<List<TEntity>> CheckIfInDatabase(IEnumerable<TEntity> entities);

        #region Bulk Operations

        /// <summary>
        /// Bulk insert entities (optimized for large datasets)
        /// </summary>
        public Task BulkInsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Bulk update entities (optimized for large datasets)
        /// </summary>
        public Task BulkUpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Bulk delete entities (optimized for large datasets)
        /// </summary>
        public Task BulkDeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Bulk delete entities by IDs (optimized for large datasets)
        /// </summary>
        public Task BulkDeleteByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute delete operation based on a predicate
        /// </summary>
        public Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute update operation based on a predicate
        /// </summary>
        public Task<int> ExecuteUpdateAsync(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TEntity>> updateExpression,
            CancellationToken cancellationToken = default);

        #endregion
    }
}
