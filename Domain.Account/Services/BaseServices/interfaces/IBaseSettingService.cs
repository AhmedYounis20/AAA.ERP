using Shared.BaseEntities;
using Shared.Responses;

namespace Shared.BaseServices.interfaces
{
    public interface IBaseSettingService<TEntity> : IBaseService<TEntity> where TEntity : BaseSettingEntity
    {
        public Task<ApiResponse> SearchByName(string name);
    }
}
