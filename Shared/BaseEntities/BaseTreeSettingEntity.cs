using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Account.Models.BaseEntities;

public class BaseTreeSettingEntity<TEntity> : BaseSettingEntity where TEntity : BaseSettingEntity
{
    public Guid? ParentId { get; set; }
    public NodeType NodeType { get; set; }
    [NotMapped]
    public List<TEntity>? Children { get; set; }
}