using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.BuildingBlocks.Domain.Entities;

public class BaseTreeEntity<TEntity> : BaseEntity where TEntity : BaseEntity
{
    public Guid? ParentId { get; set; }

    [NotMapped] public List<TEntity>? Children { get; set; }
}