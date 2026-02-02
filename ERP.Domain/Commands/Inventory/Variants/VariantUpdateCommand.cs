using System.Text.Json.Serialization;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Dtos.Inventory;
using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace ERP.Domain.Commands.Inventory.Variants;

public class VariantUpdateCommand : BaseSettingUpdateCommand<Variant>
{
    public string? Description { get; set; }
    public Guid ProductId { get; set; }
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
    public List<Guid> ManufacturerCompaniesIds { get; set; } = [];
    public List<VariantSellingPriceDiscountDto> SellingPriceDiscounts { get; set; } = [];
    public List<VariantPackingUnitDto> PackingUnits { get; set; } = [];
    public List<VariantAttachmentDto> VariantAttachments { get; set; } = [];
    public bool ApplyProductChanges { get; set; }
}
