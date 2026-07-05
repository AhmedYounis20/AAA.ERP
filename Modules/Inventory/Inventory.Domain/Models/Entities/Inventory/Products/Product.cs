using Inventory.Domain.Models.Entities.ExpiryLevels;
using Inventory.Domain.Models.Entities.InventoryThresholds;
using ERP.BuildingBlocks.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Inventory.Domain.Models.Entities.Items;
using Inventory.Domain.Models.Entities.Variants;

namespace Inventory.Domain.Models.Entities.Products;

public class Product : BaseTreeSettingEntity<Product>
{
    [NotMapped]
    public ProductCode? Code { get => ProductCodes.FirstOrDefault(e => e.CodeType == ProductCodeType.Code); }
    [NotMapped]
    public ProductCode? Gs1Code { get => ProductCodes.FirstOrDefault(e => e.CodeType == ProductCodeType.GS1); }
    [NotMapped]
    public ProductCode? EGSCode { get => ProductCodes.FirstOrDefault(e => e.CodeType == ProductCodeType.EGS); }
    [NotMapped]
    public List<ProductCode> BarCodes { get => ProductCodes.Where(e => e.CodeType == ProductCodeType.BarCode).ToList(); }

    public ProductType? ProductType { get; set; }
    public InventoryTrackingType? TrackedBy { get; set; }

    public decimal MaxDiscount { get; set; }
    public decimal ConditionalDiscount { get; set; }
    public decimal DefaultDiscount { get; set; }
    public DiscountType DefaultDiscountType { get; set; }
    public bool IsDiscountBasedOnSellingPrice { get; set; }

    public bool IsSales { get; set; }
    public bool IsPurchase { get; set; }
    public string? Model { get; set; }
    public string? Version { get; set; }
    public string? CountryOfOrigin { get; set; }

    // Navigation properties
    public List<ProductCode> ProductCodes { get; set; } = [];
    public List<ProductSupplier> ProductSuppliers { get; set; } = [];
    public List<ProductSalesTax> ProductSalesTaxes { get; set; } = [];
    public List<ProductPurchaseTax> ProductPurchaseTaxes { get; set; } = [];
    public List<ProductCostCenter> ProductCostCenters { get; set; } = [];
    public List<ProductManufacturerCompany> ProductManufacturerCompanies { get; set; } = [];
    public List<ProductAttachment> ProductAttachments { get; set; } = [];
    public List<Variant> Variants { get; set; } = [];
    public List<ProductAttributeDefinition> ProductAttributeDefinitions { get; set; } = [];

    // Template properties for variants
    public List<ProductSellingPriceDiscount> ProductSellingPriceDiscounts { get; set; } = [];
    public List<ProductPackingUnit> ProductPackingUnits { get; set; } = [];

    /// <summary>Inventory threshold scope applied to all inventory thresholds for this product.</summary>
    public InventoryThresholdScope? InventoryThresholdScope { get; set; }
    /// <summary>Branch applied to all inventory thresholds when scope is Branch; required when InventoryThresholdScope = Branch.</summary>
    public Guid? InventoryThresholdBranchId { get; set; }

    public List<InventoryThreshold> InventoryThresholds { get; set; } = [];
    public List<ExpiryLevel> ExpiryLevels { get; set; } = [];
}
