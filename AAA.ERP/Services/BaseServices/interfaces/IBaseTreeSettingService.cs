using System.Threading.Tasks;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Responses;
using AAA.ERP.Services.BaseServices.interfaces;

namespace AAA.ERP.Services.BaseServices.Interfaces;

public interface IBaseTreeSettingService<TEntity> : IBaseSettingService<TEntity> where TEntity : BaseTreeSettingEntity<TEntity>
{
    Task<List<TEntity>> GetLevel(int level = 0);
    Task<List<TEntity>> GetChildren(Guid id, int level = 0);
}