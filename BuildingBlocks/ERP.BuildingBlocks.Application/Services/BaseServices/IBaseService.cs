using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Shared.BaseEntities;
using Shared.DTOs;
using Shared.Responses;
using System.Linq.Expressions;

namespace ERP.Application.Services.BaseServices
{
    public interface IBaseService<TEntity, in TCreateCommand, in TUpdateCommand>
        where TEntity : BaseEntity
        where TCreateCommand : BaseCreateCommand<TEntity>
        where TUpdateCommand : BaseUpdateCommand<TEntity>
    {
        public Task<ApiResponse<TEntity>> Create(TCreateCommand entity, bool isValidate = true);
        public Task<ApiResponse<IEnumerable<TEntity>>> Create(List<TEntity> entities, bool isValidate = true);

        public Task<ApiResponse<TEntity>> ReadById(Guid id);
        public Task<ApiResponse<IEnumerable<TEntity>>> ReadAll();
        
        /// <summary>
        /// Gets paginated results with filtering support
        /// </summary>
        public Task<ApiResponse<PaginatedResult<TEntity>>> ReadAllPaginated(
            BaseFilterDto filter,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets paginated results with custom filter expression
        /// </summary>
        public Task<ApiResponse<PaginatedResult<TEntity>>> ReadAllPaginated(
            BaseFilterDto filter,
            Expression<Func<TEntity, bool>>? additionalFilter = null,
            CancellationToken cancellationToken = default);

        public Task<ApiResponse<TEntity>> Update(TUpdateCommand entity, bool isValidate = true);
        public Task<ApiResponse<IEnumerable<TEntity>>> Update(List<TEntity> entities, bool isValidate = true);

        public Task<ApiResponse<TEntity>> Delete(TEntity entity, bool isValidate = true);
        public Task<ApiResponse<IEnumerable<TEntity>>> Delete(List<TEntity> entities, bool isValidate = true);
        public Task<ApiResponse<TEntity>> Delete(Guid id, bool isValidate = true);
    }
}
