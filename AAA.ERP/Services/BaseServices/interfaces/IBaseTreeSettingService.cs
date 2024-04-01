using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Responses;

namespace AAA.ERP.Services.BaseServices.interfaces
{
    public interface IBaseTreeSettingService<TEntity> : IBaseTreeService<TEntity> where TEntity : BaseTreeSettingEntity
    {
        public Task<ApiResponse> SearchByName(string name);
    }
}