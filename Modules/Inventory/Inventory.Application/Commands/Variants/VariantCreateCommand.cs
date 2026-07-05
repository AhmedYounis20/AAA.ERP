using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using Inventory.Domain.Models.Dtos;
using Inventory.Domain.Models.Entities.InventoryThresholds;
using Inventory.Domain.Models.Entities.Variants;
using Inventory.Domain.Models.Entities.Items;

namespace Inventory.Application.Commands.Variants;

public class VariantCreateCommand : BaseSettingCreateCommand<Variant>
{
    public string? Description { get; set; }
    public Guid ProductId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Gs1Code { get; set; }
    public string? EGSCode { get; set; }
    public List<string> BarCodes { get; set; } = [];
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
    public List<Guid> SuppliersIds { get; set; } = [];
    public List<Guid> SalesTaxIds { get; set; } = [];
    public List<Guid> PurchaseTaxIds { get; set; } = [];
    public List<VariantCostCenterDto> CostCenters { get; set; } = [];
    public List<Guid> ManufacturerCompaniesIds { get; set; } = [];
    public List<VariantSellingPriceDiscountDto> SellingPriceDiscounts { get; set; } = [];
    public List<VariantPackingUnitDto> PackingUnits { get; set; } = [];
    public List<VariantAttachmentDto> VariantAttachments { get; set; } = [];
    public List<VariantAttributeValueDto> VariantAttributeValues { get; set; } = [];
    public bool IsDefault { get; set; }
    public bool IsArchived { get; set; }
    public bool ApplyProductChanges { get; set; }
    public InventoryThresholdScope? InventoryThresholdScope { get; set; }
    public Guid? InventoryThresholdBranchId { get; set; }
    public List<InventoryThresholdDto> InventoryThresholds { get; set; } = [];
    public List<ExpiryLevelDto> ExpiryLevels { get; set; } = [];
}
