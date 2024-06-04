using AAA.ERP.Models.BaseEntities;

namespace AAA.ERP.Models.Entities.SubLeadgers;

public class SubLeadgerBaseEntity<TEntity> : BaseTreeSettingEntity<TEntity> where TEntity : BaseSettingEntity
{
    public Guid ChartOfAccountId { get; set; }
    public string Notes { get; set; }
}