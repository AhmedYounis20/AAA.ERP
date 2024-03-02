using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Responses;

namespace AAA.ERP.Services.BaseServices.interfaces
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        public Task<ApiResponse> Create(TEntity entity);
        public Task<ApiResponse> Create(List<TEntity> entities);

        public Task<ApiResponse> ReadById(Guid id);
        public Task<ApiResponse> ReadAll();

        public Task<ApiResponse> Update(TEntity entity);
        public Task<ApiResponse> Update(List<TEntity> entities);

        public Task<ApiResponse> Delete(TEntity entity);
        public Task<ApiResponse> Delete(List<TEntity> entities);
        public Task<ApiResponse> Delete(Guid id);
    }
}
