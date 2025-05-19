using ERP.Application.Repositories.BaseRepositories;
using Shared.BaseEntities;

namespace ERP.Application.Repositories.SubLeadgers;

public interface IBaseSubLeadgerRepository<TEntity> : IBaseTreeSettingRepository<TEntity> where TEntity : BaseTreeSettingEntity<TEntity>
{ }