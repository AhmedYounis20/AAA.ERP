using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.Services.BaseServices.interfaces
{
    public interface IBaseSettingService<TEntity> : IBaseService<TEntity> where TEntity : BaseSettingEntity
    {
        public Task<ApiResponse<IEnumerable<TEntity>>> SearchByName(string name);
    }
}
