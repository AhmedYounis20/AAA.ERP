using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Account.SubLeadgers;

public class SubLeadgerBaseEntity<TEntity> : BaseTreeSettingEntity<TEntity> where TEntity : BaseSettingEntity
{
    public Guid? ChartOfAccountId { get; set; }
    public virtual ChartOfAccount? ChartOfAccount { get; set; }
    public string? Notes { get; set; }
}