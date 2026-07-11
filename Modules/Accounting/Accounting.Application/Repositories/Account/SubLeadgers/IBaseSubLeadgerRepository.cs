using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;

namespace Accounting.Application.Repositories.Account.SubLeadgers;

/// <summary>
/// Repository contract for base sub leadger persistence.
/// </summary>
public interface IBaseSubLeadgerRepository<TEntity> : IBaseTreeSettingRepository<TEntity> where TEntity : BaseTreeSettingEntity<TEntity>
{ }
