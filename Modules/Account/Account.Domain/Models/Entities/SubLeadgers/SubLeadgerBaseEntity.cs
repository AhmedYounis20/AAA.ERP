using Account.Domain.Models.Entities.ChartOfAccounts;

namespace Account.Domain.Models.Entities.SubLeadgers;

public class SubLeadgerBaseEntity<TEntity> : BaseTreeSettingEntity<TEntity> where TEntity : BaseSettingEntity
{
    public Guid? ChartOfAccountId { get; set; }
    public virtual ChartOfAccount? ChartOfAccount { get; set; }
    public string? Notes { get; set; }
}
