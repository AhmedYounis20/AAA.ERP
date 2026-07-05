using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Inventory.Domain.Models.Dtos;
using Inventory.Domain.Models.Entities.InventoryThresholds;
using Inventory.Domain.Models.Entities.Products;
using Inventory.Domain.Models.Entities.Items;

namespace Inventory.Application.Commands.Products;

public class ProductUpdateCommand : BaseTreeSettingUpdateCommand<Product>
{
    public string Code { get; set; } = string.Empty;
    public ProductAttachmentDto? ProductImage { get; set; }
    public string? Gs1Code { get; set; }
    public string? EGSCode { get; set; }
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
    public List<string> BarCodes { get; set; } = [];
    public List<Guid> SuppliersIds { get; set; } = [];
    public List<Guid> SalesTaxIds { get; set; } = [];
    public List<Guid> PurchaseTaxIds { get; set; } = [];
    public List<ProductCostCenterDto> CostCenters { get; set; } = [];
    public List<Guid> ManufacturerCompaniesIds { get; set; } = [];
    public List<ProductAttachmentDto> ProductAttachments { get; set; } = [];
    public List<ProductAttributeDefinitionDto> ProductAttributeDefinitions { get; set; } = [];
    public List<ProductSellingPriceDiscountDto> SellingPriceDiscounts { get; set; } = [];
    public List<ProductPackingUnitDto> PackingUnits { get; set; } = [];
    public List<VariantCombinationDto> VariantCombinations { get; set; } = [];
    public InventoryThresholdScope? InventoryThresholdScope { get; set; }
    public Guid? InventoryThresholdBranchId { get; set; }
    public List<InventoryThresholdDto> InventoryThresholds { get; set; } = [];
    public List<ExpiryLevelDto> ExpiryLevels { get; set; } = [];
}
