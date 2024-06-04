using System.ComponentModel.DataAnnotations.Schema;
using Domain.Account.Models.BaseEntities;

namespace Domain.Account.Models.BaseEntities;

public class BaseTreeEntity<TEntity> : BaseEntity where TEntity : BaseEntity
{
    public Guid? ParentId { get; set; }

    [NotMapped] public List<TEntity>? Children { get; set; }
}