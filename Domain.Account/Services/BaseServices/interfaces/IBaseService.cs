using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.Services.BaseServices.interfaces
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        public Task<ApiResponse<TEntity>> Create(TEntity entity,bool isValidate = true);
        public Task<ApiResponse<IEnumerable<TEntity>>> Create(List<TEntity> entities, bool isValidate = true);

        public Task<ApiResponse<TEntity>> ReadById(Guid id);
        public Task<ApiResponse<IEnumerable<TEntity>>> ReadAll();

        public Task<ApiResponse<TEntity>> Update(TEntity entity, bool isValidate = true);
        public Task<ApiResponse<IEnumerable<TEntity>>> Update(List<TEntity> entities, bool isValidate = true);

        public Task<ApiResponse<TEntity>> Delete(TEntity entity, bool isValidate = true);
        public Task<ApiResponse<IEnumerable<TEntity>>> Delete(List<TEntity> entities, bool isValidate = true);
        public Task<ApiResponse<TEntity>> Delete(Guid id, bool isValidate = true);
    }
}
