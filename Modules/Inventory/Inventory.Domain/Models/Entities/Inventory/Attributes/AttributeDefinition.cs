using Inventory.Domain.Models.Entities.AttributeDefinitions;
using Inventory.Domain.Models.Entities.Products;
using Inventory.Domain.Models.Entities.Variants;
using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Attributes;

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