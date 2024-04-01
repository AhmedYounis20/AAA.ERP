namespace AAA.ERP.Models.BaseEntities;

public class BaseTreeEntity :BaseEntity
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
}