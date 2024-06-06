using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.BaseRepositories.Interfaces;
using Shared.BaseEntities;

namespace Domain.Account.Repositories.Interfaces.SubLeadgers;

public interface IBaseSubLeadgerRepository<TEntity> : IBaseTreeSettingRepository<TEntity> where TEntity : BaseTreeSettingEntity<TEntity>
{ }