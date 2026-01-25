using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;
using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Domain.Models.Entities.Inventory.Variants;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Attributes;

public class AttributeDefinition : BaseSettingEntity
{
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    // public string? Code { get; set; }
    // Navigation properties
    public ICollection<AttributeValue> PredefinedValues { get; set; } = new List<AttributeValue>();
    public ICollection<VariantAttributeValue> VariantAttributeValues { get; set; } = new List<VariantAttributeValue>();
    public ICollection<ProductAttributeDefinition> ProductAttributes { get; set; } = new List<ProductAttributeDefinition>();
}