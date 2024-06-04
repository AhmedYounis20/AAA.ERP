using Shared.BaseEntities;
using Shared.Responses;

namespace Shared.BaseServices.interfaces
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        public Task<ApiResponse> Create(TEntity entity,bool isValidate = true);
        public Task<ApiResponse> Create(List<TEntity> entities, bool isValidate = true);

        public Task<ApiResponse> ReadById(Guid id);
        public Task<ApiResponse> ReadAll();

        public Task<ApiResponse> Update(TEntity entity, bool isValidate = true);
        public Task<ApiResponse> Update(List<TEntity> entities, bool isValidate = true);

        public Task<ApiResponse> Delete(TEntity entity, bool isValidate = true);
        public Task<ApiResponse> Delete(List<TEntity> entities, bool isValidate = true);
        public Task<ApiResponse> Delete(Guid id, bool isValidate = true);
    }
}
