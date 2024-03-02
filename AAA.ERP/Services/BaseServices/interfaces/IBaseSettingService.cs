using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Responses;

namespace AAA.ERP.Services.BaseServices.interfaces
{
    public interface IBaseSettingService<TEntity> : IBaseService<TEntity> where TEntity : BaseSettingEntity
    {
        public Task<ApiResponse> SearchByName(string name);
    }
}
