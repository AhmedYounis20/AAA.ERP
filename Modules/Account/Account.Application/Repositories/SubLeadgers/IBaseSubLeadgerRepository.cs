using Shared.Application.Repositories;
using Shared.Domain.BaseEntities;

namespace Account.Application.Repositories.SubLeadgers;

public interface IBaseSubLeadgerRepository<TEntity> : IBaseTreeSettingRepository<TEntity> where TEntity : BaseTreeSettingEntity<TEntity>
{ }