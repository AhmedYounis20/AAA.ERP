using Inventory.Domain.Models.Entities.ExpiryLevels;
using Inventory.Domain.Models.Entities.InventoryThresholds;
using ERP.BuildingBlocks.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Inventory.Domain.Models.Entities;

namespace Inventory.Domain.Models.Entities.Variants;

public class Variant : BaseEntity
{
    public string? Description { get; set; }

    public Guid ProductId { get; set; }
    public Products.Product? Product { get; set; }

    [NotMapped]
    public VariantCode? Code { get => VariantCodes.FirstOrDefault(e => e.CodeType == VariantCodeType.Code); }
    [NotMapped]
    public VariantCode? Gs1Code { get => VariantCodes.FirstOrDefault(e => e.CodeType == VariantCodeType.GS1); }
    [NotMapped]
    public VariantCode? EGSCode { get => VariantCodes.FirstOrDefault(e => e.CodeType == VariantCodeType.EGS); }
    [NotMapped]
    public List<VariantCode> BarCodes { get => VariantCodes.Where(e => e.CodeType == VariantCodeType.BarCode).ToList(); }

    // Variant-specific properties (moved from Item)
    public decimal MaxDiscount { get; set; }
    public decimal ConditionalDiscount { get; set; }
    public decimal DefaultDiscount { get; set; }
    public DiscountType DefaultDiscountType { get; set; }
    public bool IsDiscountBasedOnSellingPrice { get; set; }
    public string? Model { get; set; }
    public string? Version { get; set; }
    public string? CountryOfOrigin { get; set; }
    public bool IsSales { get; set; }
    public bool IsPurchase { get; set; }
    public bool IsArchived { get; set; }
    public bool IsDefault { get; set; }
    public bool IsUsed { get; set; }
    public bool ApplyProductChanges { get; set; }

    // Navigation properties
    public List<VariantCode> VariantCodes { get; set; } = [];
    public List<VariantSupplier> VariantSuppliers { get; set; } = [];
    public List<VariantSalesTax> VariantSalesTaxes { get; set; } = [];
    public List<VariantPurchaseTax> VariantPurchaseTaxes { get; set; } = [];
    public List<VariantCostCenter> VariantCostCenters { get; set; } = [];
    public List<VariantManufacturerCompany> VariantManufacturerCompanies { get; set; } = [];
    public List<VariantSellingPriceDiscount> VariantSellingPriceDiscounts { get; set; } = [];
    public List<VariantPackingUnit> VariantPackingUnitPrices { get; set; } = [];
    public List<VariantAttachment> VariantAttachments { get; set; } = [];
    public List<VariantAttributeValue> VariantAttributeValues { get; set; } = [];

    /// <summary>Inventory threshold scope applied to all inventory thresholds for this variant.</summary>
    public InventoryThresholdScope? InventoryThresholdScope { get; set; }
    /// <summary>Branch applied to all inventory thresholds when scope is Branch; required when InventoryThresholdScope = Branch.</summary>
    public Guid? InventoryThresholdBranchId { get; set; }

    public List<InventoryThreshold> InventoryThresholds { get; set; } = [];
    public List<ExpiryLevel> ExpiryLevels { get; set; } = [];
}