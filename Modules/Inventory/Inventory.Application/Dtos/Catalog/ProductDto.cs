using ERP.BuildingBlocks.Domain.Entities;
using System.Text.Json.Serialization;
using ERP.BuildingBlocks.Application.DTOs;
using Inventory.Domain.Models.Entities.InventoryThresholds;
using Inventory.Domain.Models.Entities.Products;
using Inventory.Domain.Models.Entities;

namespace Inventory.Application.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
    public Guid? ParentId { get; set; }
    public NodeType NodeType { get; set; }

    public string Code
    {
        get => ProductCodes.FirstOrDefault(e => e.CodeType == ProductCodeType.Code)?.Code ?? string.Empty;
        set { }
    }

    public string? Gs1Code
    {
        get => ProductCodes.FirstOrDefault(e => e.CodeType == ProductCodeType.GS1)?.Code ?? string.Empty;
        set { }
    }

    public string? EGSCode
    {
        get => ProductCodes.FirstOrDefault(e => e.CodeType == ProductCodeType.EGS)?.Code ?? string.Empty;
        set { }
    }
    public List<string> BarCodes { get => ProductCodes.Where(e => e.CodeType == ProductCodeType.BarCode).Select(e => e.Code).ToList(); }

    public ProductType? ProductType { get; set; }
    public decimal MaxDiscount { get; set; }
    public decimal ConditionalDiscount { get; set; }
    public decimal DefaultDiscount { get; set; }
    public DiscountType DefaultDiscountType { get; set; }
    public bool IsDiscountBasedOnSellingPrice { get; set; }
    public string? Model { get; set; }
    public string? Version { get; set; }
    public string? CountryOfOrigin { get; set; }
    public List<Guid> SuppliersIds { get; set; } = [];
    public List<Guid> SalesTaxIds { get; set; } = [];
    public List<Guid> PurchaseTaxIds { get; set; } = [];
    public List<ProductCostCenterDto> CostCenters { get; set; } = [];
    public List<Guid> ManufacturerCompaniesIds { get; set; } = [];
    public List<ProductAttachmentDto> ProductAttachments { get; set; } = [];
    public AttachmentDto? ProductPicture { get; set; }
    public List<ProductAttributeDefinitionDto> ProductAttributeDefinitions { get; set; } = [];
    public List<ProductSellingPriceDiscountDto> SellingPriceDiscounts { get; set; } = [];
    public List<ProductPackingUnitDto> PackingUnits { get; set; } = [];
    /// <summary>Inventory threshold scope for all inventory thresholds; user chooses once per product.</summary>
    public InventoryThresholdScope? InventoryThresholdScope { get; set; }
    /// <summary>Branch for all inventory thresholds when scope is Branch; user chooses once per product.</summary>
    public Guid? InventoryThresholdBranchId { get; set; }
    public List<InventoryThresholdDto> InventoryThresholds { get; set; } = [];
    public List<ExpiryLevelDto> ExpiryLevels { get; set; } = [];

    public Dictionary<Guid, HashSet<Guid>> ProductDefinitionsValues
    {
        get
        {
            return VariantCombinations
                .SelectMany(v => v.AttributeValues)
                .GroupBy(av => av.Key)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.Value).ToHashSet()
                );
        }
    }

    public bool HasVariants { get; set; }
    public int VariantsCount { get; set; }
    public List<VariantCombinationDto> VariantCombinations { get; set; } = [];
    [JsonIgnore]
    public List<ProductCode> ProductCodes { get; set; } = [];

    public string? CreatedByName { get; set; }
    public string? CreatedByNameSecondLanguage { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? ModifiedByName { get; set; }
    public string? ModifiedByNameSecondLanguage { get; set; }
    public DateTime ModifiedAt { get; set; }
}
