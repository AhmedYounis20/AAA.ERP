using System.ComponentModel.DataAnnotations.Schema;

namespace AAA.ERP.Models.BaseEntities;

public class BaseTreeSettingEntity<TEntity> : BaseSettingEntity where TEntity : BaseSettingEntity
{
    public Guid? ParentId { get; set; }
    [NotMapped]
    public List<TEntity>? Children { get; set; }
}