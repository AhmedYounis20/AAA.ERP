using Shared.BaseEntities;

namespace ERP.Domain.Models.Dtos.Inventory;

public class ProductAttributeDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid AttributeDefinitionId { get; set; }
    public string AttributeName { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public int SortOrder { get; set; }

    public string? CreatedByName { get; set; }
    public string? CreatedByNameSecondLanguage { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? ModifiedByName { get; set; }
    public string? ModifiedByNameSecondLanguage { get; set; }
    public DateTime ModifiedAt { get; set; }
}


