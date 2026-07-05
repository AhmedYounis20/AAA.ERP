using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Shared.BaseEntities;

namespace Accounting.Application.Repositories.Account.SubLeadgers;

public interface IBaseSubLeadgerRepository<TEntity> : IBaseTreeSettingRepository<TEntity> where TEntity : BaseTreeSettingEntity<TEntity>
{ }